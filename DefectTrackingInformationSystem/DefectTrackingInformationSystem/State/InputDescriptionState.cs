using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DefectTrackingInformationSystem.State
{
    public class InputDescriptionState : BaseDefectState
    {
        private readonly TelegramBotClient _botClient;
        public override string Name => CommandNames.InputDecscriptionCommand;

        public InputDescriptionState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
        }

        public override async Task ExecuteStateAsync(Update update)
        {
            
            var message = update.Message;
            if (message.Text != null)
            {
                defect.Description = message.Text;

                var messageText = "Виберіть дію...";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: GetButtons());
            }
            else
            {
                var messageText = "Повторіть ще раз, тут має бути текст. ";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
                throw new Exception();
            }
           
        }





        public IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            (new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{ new KeyboardButton("Завантажити фото дефекту") },
                    new List<KeyboardButton>{new KeyboardButton("Завершити")},
                    new List<KeyboardButton>{new KeyboardButton("Перейти в меню")},
            });

        }

    }
}
