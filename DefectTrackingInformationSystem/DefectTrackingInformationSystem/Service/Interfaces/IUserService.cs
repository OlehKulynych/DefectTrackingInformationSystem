using Telegram.Bot.Types;
using User = DefectTrackingInformationSystem.Models.User;

namespace DefectTrackingInformationSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Update update);
        Task<User> CreateUserAsync(Update update);

        Task<IList<string>> GetRolesAsync(User user);
        Task<bool> IsInRoleAsync(User user, string roleName);
    }
}
