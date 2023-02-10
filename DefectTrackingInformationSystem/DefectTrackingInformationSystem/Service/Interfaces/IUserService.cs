using Telegram.Bot.Types;
using User = DefectTrackingInformationSystem.Models.User;

namespace DefectTrackingInformationSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetOrCreateUserAsync(Update update);
    }
}
