using DefectTrackingInformationSystem.Commands;
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
            var user = await _userService.GetOrCreateUserAsync(update);
            //var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            //{
                
            //        new KeyboardButton("Повідомити про дефект"),
            //        new KeyboardButton("Переглянути наявні дефекти")
                
            //});

            await _botClient.SendTextMessageAsync(user.ChatId, "Це бот для того, щоб ви могли повідомити про знайдену вами поломку й техперсонал міг швидко її вирішити)",
                ParseMode.Markdown, replyMarkup: GetButtons());
        }

        public IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            ( new List<List<KeyboardButton>> {

                    new List<KeyboardButton>{ new KeyboardButton("Повідомити про дефект") },
                    new List<KeyboardButton>{new KeyboardButton("Переглянути наявні дефекти")},
                    new List<KeyboardButton>{new KeyboardButton("Виправити дефекти") }
            } );

        }

    }
}
