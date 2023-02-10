using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.Commands
{
    public class AddDefectCommand: BaseCommand
    {
        private readonly TelegramBotClient _botClient;

        public AddDefectCommand(TelegramBotService telegramBotService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
        }
        public override string Name => CommandNames.AddDefectCommand;

        public override async Task ExecuteAsync(Update update)
        {
            const string message = "Для додавання нового дефекту введіть потрібну інформацію в такому вигляді : \n\nНомер кімнати з дефектом: ";

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message, ParseMode.Markdown);
        }

    }
}
