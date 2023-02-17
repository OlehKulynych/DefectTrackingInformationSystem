using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.State
{
    public class StartInputImageDefectState : BaseDefectState
    {
        private readonly TelegramBotClient _botClient;

        public StartInputImageDefectState(TelegramBotService botService)
        {
            _botClient = botService.GetTelegramBot().Result;

        }

        public override string Name => CommandNames.StartInputImageDefectCommand;

        public override async Task ExecuteStateAsync(Update update)
        {

            var message = update.Message;
            var messageText = "Завантажте фото: ";
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());

        }
    }
}
