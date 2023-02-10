using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.State
{
    public class FinishInputDefectState : State
    {
        public override string Name => CommandNames.FinishInputDefectCommand;
        private TelegramBotClient _botClient;
        private DataBaseContext _dataBaseContext;
        public FinishInputDefectState(TelegramBotService botService, DataBaseContext dataBaseContext)
        {
            _botClient = botService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }
        public override async Task ExecuteStateAsync(Update update)
        {
            var message = update.Message;

        }
    }
}
