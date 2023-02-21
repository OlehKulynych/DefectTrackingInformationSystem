using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Service.Interfaces;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using DefectTrackingInformationSystem.Models;
using System.Net;

namespace DefectTrackingInformationSystem.State
{
    public class StartState : BaseDefectState
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public StartState(IUserService userService, TelegramBotService telegramBotService)
        {
            _userService = userService;
            _botClient = telegramBotService.GetTelegramBot().Result;
        }


        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            try
            {
                var user = await _userService.GetUserAsync(update);
                if (user == null)
                {
                    await _userService.CreateUserAsync(update);
                }

                await _botClient.SendTextMessageAsync(user.ChatId, "Це бот для того, щоб ви могли повідомити про знайдену вами поломку й техперсонал міг швидко її вирішити)",
                    ParseMode.Markdown, replyMarkup: GetButtons(user).Result);
            }
            catch(Exception ex)
            {
                var messageText = $"Помилка в StartState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
            }
        }

        public async Task<IReplyMarkup> GetButtons(Models.User user)
        {
            List<List<KeyboardButton>> keyboardButtons = new List<List<KeyboardButton>>();

            if(await _userService.IsInRoleAsync(user, RoleNames.RepairEmployee))
            {
                keyboardButtons.Add(new List<KeyboardButton> { new KeyboardButton("Переглянути наявні дефекти") });
                keyboardButtons.Add(new List<KeyboardButton> { new KeyboardButton("Виправити дефекти") });
            }
            else if(await _userService.IsInRoleAsync(user, RoleNames.TechnicalStaff))
            {
                keyboardButtons.Add(new List<KeyboardButton> { new KeyboardButton("Повідомити про дефект") });
            }

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            return replyKeyboardMarkup;
        }

    }
}
