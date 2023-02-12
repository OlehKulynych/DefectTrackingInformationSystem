using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public class InputDefectState: BaseDefectState
    {
        private readonly TelegramBotClient _botClient;

        

        public override string Name => CommandNames.InputDefectCommand;

        public InputDefectState(TelegramBotService telegramBotService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
        }
      
        public override async Task ExecuteStateAsync(Update update)
        {
            const string message = "Для додавання нового дефекту введіть потрібну інформацію в такому вигляді : \n\nНомер кімнати з дефектом: ";

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message, ParseMode.Markdown);
        }

    }
}
