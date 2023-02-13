using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DefectTrackingInformationSystem.State
{
    public class InputImageDefectState : BaseDefectState
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;

        public InputImageDefectState(TelegramBotService botService, DataBaseContext dataBaseContext)
        {
            _botClient = botService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }
        public override string Name => CommandNames.InputImageDefectCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            try
            {
                var message = update.Message;
                if (message.Photo != null)
                {
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await _botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;

                    defect.ImageString = filePath;

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фото завантажено)", ParseMode.Markdown, replyMarkup: GetButtons());
                }
                else
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ви надсилаєте не фото, можливо ви надсилаєте файлом?", ParseMode.Markdown);
                }
            }
            catch(Exception ex)
            {
                var messageText = $"Помилка в InputImageDefectState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
            }           
        }

        public IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            (new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{new KeyboardButton("Завершити")}
            });
        }
    }
}
