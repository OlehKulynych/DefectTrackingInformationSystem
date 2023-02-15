using DTIS.Shared.Models;
using DTIS.WebApi.Data;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DTIS.WebApi.Repositories;

public class DefectRepository : IDefectRepository
{
    private readonly DataContext _context;

    public DefectRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Defect>> GetAllDefects()
    {
        return await _context.Defects.ToListAsync();
    }

    public async Task<Defect?> GetDefectById(int id)
    {
        return await _context.Defects.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddDefect(Defect defect)
    {
        _context.Defects.Add(defect);
        var created = await _context.SaveChangesAsync();

        return created > 0;
    }
 
    public async Task<bool> UpdateDefect(Defect defect)
    {
        _context.Defects.Update(defect);
        var updated = await _context.SaveChangesAsync();

        return updated > 0;
    }

    public async Task<bool> DeleteDefect(int id)
    {
        var defect = await GetDefectById(id);

        if (defect == null)
        {
            return false;
        }

        _context.Defects.Remove(defect);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}
