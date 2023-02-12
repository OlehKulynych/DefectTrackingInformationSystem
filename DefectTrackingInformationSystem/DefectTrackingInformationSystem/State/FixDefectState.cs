using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DefectTrackingInformationSystem.State
{
    public class FixDefectState : BaseState
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;
        public FixDefectState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }

        public override string Name => CommandNames.FixDefectCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Невирішені дефекти: \n");

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
            message.Clear();

            var defects = _dataBaseContext.Defectes.Where(x => x.isClosed == false).ToList();
            if(defects.Count() != 0)
            {
                foreach (var el in defects)
                {

                    message.AppendLine($" --- \n Id: {el.Id} \n Номер кімнати: {el.RoomNumber} \n Опис: {el.Description}");

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown, replyMarkup: GetInlineButton(el.Id));
                    message.Clear();
                }
            }
            else
            {

                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Немає наявних дефектів, можна відпочити)))", ParseMode.Markdown);
            }
           

        }

        private IReplyMarkup GetInlineButton(int Id)
        {
            return new InlineKeyboardMarkup( new InlineKeyboardButton("Fix") { CallbackData = Id.ToString()});
        }
    }
}
