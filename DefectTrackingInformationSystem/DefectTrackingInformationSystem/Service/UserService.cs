using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DefectTrackingInformationSystem.Models.User;

namespace DefectTrackingInformationSystem.Service
{
    public class UserService : IUserService
    {
        private readonly DataBaseContext _context;

        public UserService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(Update update)
        {
            var userChatId = update.Type switch
            {
                UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat.Id.ToString(),

                UpdateType.Message => update.Message.Chat.Id.ToString()
            };


            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == userChatId);

            if (user != null)
            {
                return user; 
            }
            else
            {
                return null;
            }

        }

        public async Task<User> CreateUserAsync(Update update)
        {
            var newUser = update.Type switch
            {
                UpdateType.CallbackQuery => new User
                {
                    Id = update.CallbackQuery.Message.Chat.Id.ToString(),
                    ChatId = update.CallbackQuery.Message.Chat.Id.ToString(),
                    FirstName = update.CallbackQuery.Message.From.FirstName,
                    LastName = update.CallbackQuery.Message.From.LastName
                },
                UpdateType.Message => new User
                {
                    Id = update.Message.Chat.Id.ToString(),
                    ChatId = update.Message.Chat.Id.ToString(),
                    FirstName = update.Message.Chat.FirstName,
                    LastName = update.Message.Chat.LastName
                }
            };


            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userDb = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == user.Id);
            if (userDb != null)
            {
                var Roles = userDb.Roles.ToList();
                if (Roles != null)
                {
                    List<string> roleNames = new List<string>();
                    foreach (var r in Roles)
                    {
                        roleNames.Add(r.Name);
                    }
                    return roleNames;
                }
                return null;
            }
            return null;


        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var roles = await GetRolesAsync(user);
            return roles.Contains(roleName);
        }


        public async Task<IList<User>> GetUsersInRoleAsync(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }
            var roleDb = await _context.Roles.Include(r => r.Users).SingleOrDefaultAsync(r => r.Name == roleName);
            if (roleDb != null)
            {
                var users = roleDb.Users.ToList();

                return users;
            }
            return null;
        }
    }
}
