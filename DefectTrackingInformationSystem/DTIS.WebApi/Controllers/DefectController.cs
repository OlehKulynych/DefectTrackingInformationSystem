using DTIS.Shared.Models;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DTIS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefectController : ControllerBase
{
    private readonly IDefectRepository _defectRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DefectController(IDefectRepository defectRepository, IWebHostEnvironment webHostEnvironment)
    {
        _defectRepository = defectRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, TechnicalWorker, RepairServiceEmployee")]
    public async Task<ActionResult<List<Defect>>> GetAllDefects()
    {
        return Ok(await _defectRepository.GetAllDefectsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Defect>> GetDefectById(int id)
    {
        var defect = await _defectRepository.GetDefectByIdAsync(id);

        if (defect == null)
        {
            return NotFound();
        }

        return Ok(defect);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, TechnicalWorker")]
    public async Task<IActionResult> AddDefect([FromForm]Defect defect)
    {
        if(defect.File != null)
        {
            var addedPhoto = UploadPhoto(defect.File);

            if(string.IsNullOrEmpty(addedPhoto))
            {
                return BadRequest();
            }

            defect.ImageString = addedPhoto;
        }

        await _defectRepository.AddDefectAsync(defect);

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Administrator, TechnicalWorker")]
    public async Task<IActionResult> UpdateDefect(Defect defect)
    {
        var updateDefect = await _defectRepository.GetDefectByIdAsync(defect.Id);

        if (updateDefect == null)
        {
            return NotFound();
        }

        updateDefect.RoomNumber = defect.RoomNumber;
        updateDefect.Description = defect.Description;
        updateDefect.ImageString = defect.ImageString;
        updateDefect.isClosed = defect.isClosed;

        await _defectRepository.UpdateDefectAsync(updateDefect);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteDefect(int id)
    {
        var deleted = await _defectRepository.DeleteDefectAsync(id);

        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost("{id}")]
    [Authorize(Roles = "Administrator, RepairServiceEmployee")]
    public async Task<IActionResult> CloseDefect(int id)
    {
        var defect = await _defectRepository.GetDefectByIdAsync(id);

        if (defect == null)
        {
            return NotFound();
        }

        try
        {
            var isClosed = await _defectRepository.IsClosedDefectAsync(id);
        }
        catch (Exception)
        {
            return BadRequest("Already closed.");
        }

        await _defectRepository.CloseDefectAsync(id);

        return Ok();
    }

    private string? UploadPhoto(IFormFile photo)
    {
        var extensionPhoto = new string[] { ".jpg", ".jpeg", ".png", ".svg" };
        var resultPath = string.Empty;

        try
        {
            if (photo.Length > 0)
            {
                var path = _webHostEnvironment.WebRootPath;
                var projectPath = Directory.GetCurrentDirectory();
                path = Path.GetFullPath(Path.Combine(projectPath, @"..\")) + "\\photos\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(path + photo.FileName))
                {
                    var extension = Path.GetExtension(path + photo.FileName);

                    if (extension == null || !extensionPhoto.Contains(extension))
                    {
                        return null;
                    }

                    resultPath = path + photo.FileName;
                    photo.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
        }
        catch (Exception)
        {
            return null;
        }

        return resultPath;
    }
}
