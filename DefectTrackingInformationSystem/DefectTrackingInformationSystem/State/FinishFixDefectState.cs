using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.State
{
    public class FinishFixDefectState : BaseState
    {
        public override string Name => CommandNames.FinishFixDefectCommand;
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;
        public FinishFixDefectState(TelegramBotService botService, DataBaseContext dataBaseContext)
        {
            _botClient = botService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }
        public override async Task ExecuteStateAsync(Update update)
        {
            StringBuilder message = new StringBuilder();
           
            try
            {
                var defect = await _dataBaseContext.Defectes.Where(x => x.Id == int.Parse(update.CallbackQuery.Data)).FirstOrDefaultAsync();
                if (defect != null)
                {
                    defect.isClosed = true;

                    _dataBaseContext.Defectes.Update(defect);
                    await _dataBaseContext.SaveChangesAsync();

                    await _botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, "✅ Fix");

                    message.AppendLine($"Ви успішно виправили дефект {update.CallbackQuery.Data} , так тримати, продовжуйте в цьому ж дусі)");
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Помилка при пошуку даного дефекту...", ParseMode.Markdown);
                }
            }
            catch(Exception ex)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Помилка в FinishFixDefectState з дефектом {update.CallbackQuery.Data}...\n{ex.Message}", ParseMode.Markdown) ; 
            }                                   
        }
    }
}
