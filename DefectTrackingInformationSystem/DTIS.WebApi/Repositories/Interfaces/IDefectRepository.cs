using DTIS.Shared.Models;

namespace DTIS.WebApi.Repositories.Interfaces;

public interface IDefectRepository
{
    Task<List<Defect>> GetAllDefects();
    Task<Defect?> GetDefectById(int id);
    Task<bool> AddDefect(Defect defect);
    Task<bool> UpdateDefect(Defect defect);
    Task<bool> DeleteDefect(int id);
}
