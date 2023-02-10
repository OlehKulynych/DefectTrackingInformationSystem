using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DefectTrackingInformationSystem.Models.User;

namespace DefectTrackingInformationSystem.Service
{
    public class UserService: IUserService
    {
        private readonly DataBaseContext _context;

        public UserService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetOrCreateUserAsync(Update update)
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

            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);

            if (user != null)
            { return user; }

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        
    }
}
