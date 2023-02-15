using DTIS.Shared.Models;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DTIS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefectController : ControllerBase
{
    private readonly IDefectRepository _defectRepository;

    public DefectController(IDefectRepository defectRepository) 
    {
        _defectRepository = defectRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Defect>>> GetAllDefects()
    { 
        return Ok(await _defectRepository.GetAllDefects());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Defect>> GetDefectById(int id)
    {
        var defect = await _defectRepository.GetDefectById(id);

        if(defect == null)
        {
            return NotFound();
        }

        return Ok(defect);
    }

    [HttpPost]
    public async Task<IActionResult> AddDefect(Defect defect)
    {
        return Ok(await _defectRepository.AddDefect(defect));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDefect(Defect defect)
    {
        var updateDefect = await _defectRepository.GetDefectById(defect.Id);

        if(updateDefect == null)
        {
            return NotFound();
        }

        updateDefect.RoomNumber = defect.RoomNumber;
        updateDefect.Description = defect.Description;
        updateDefect.ImageString = defect.ImageString;
        updateDefect.isClosed = defect.isClosed;

        await _defectRepository.UpdateDefect(updateDefect);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDefect(int id)
    {
        var deleted = await _defectRepository.DeleteDefect(id);

        if(deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
