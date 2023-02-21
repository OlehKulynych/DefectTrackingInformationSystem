using DTIS.Shared.Models;

namespace DTIS.WebApi.Repositories.Interfaces;

public interface IDefectRepository
{
    Task<List<Defect>> GetAllDefectsAsync();
    Task<Defect?> GetDefectByIdAsync(int id);
    Task<bool> AddDefectAsync(Defect defect);
    Task<bool> UpdateDefectAsync(Defect defect);
    Task<bool> DeleteDefectAsync(int id);
    Task<bool> IsClosedDefectAsync(int id);
    Task<bool> CloseDefectAsync(int id);
}
