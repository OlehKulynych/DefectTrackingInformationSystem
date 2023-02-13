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
            try
            {
                var message = update.Message;
                if (message.Text != null)
                {
                    var messageText = "Завантажте фото: ";
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
                }
                else
                {
                    var messageText = "Повторіть ще раз...";
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
                }
            }
            catch(Exception ex)
            {
                var messageText = $"Помилка в StartInputImageDefectState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
            }
        }
    }
}
