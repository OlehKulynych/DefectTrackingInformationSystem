using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.State
{
    public class InputDescriptionState : State
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;
        public override string Name => CommandNames.InputDecscriptionCommand;

        public InputDescriptionState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }

        public override async Task ExecuteStateAsync(Update update)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                defect.Description = message.Text;
                defect.isClosed = false;

                _dataBaseContext.Defectes.Add(defect);
                await _dataBaseContext.SaveChangesAsync();
                
                var messageText = "Закінчуємо додавання...";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);

                defect = new Defect();
            }
            else
            {

                var messageText = "Повторіть ще раз, тут має бути текст. ";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
            }
        }
    }
}
