using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using DefectTrackingInformationSystem.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.State
{
    public class FinishInputDefectState : BaseDefectState
    {
        public override string Name => CommandNames.FinishInputDefectCommand;
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;
        private readonly IUserService _userService;

        public FinishInputDefectState(TelegramBotService botService, DataBaseContext dataBaseContext, IUserService userService)
        {
            _botClient = botService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
            _userService = userService;
        }
        public override async Task ExecuteStateAsync(Update update)
        {
            var message = update.Message;
            try
            {
                defect.isClosed = false;
                if (defect.ImageString != null)
                {
                    var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

                    await using Stream fileStream = System.IO.File.OpenWrite($@"{directory.Parent.FullName}\{defect.ImageString}");
                    await _botClient.DownloadFileAsync(
                        filePath: defect.ImageString,
                        destination: fileStream);

                }

                _dataBaseContext.Defectes.Add(defect);
                await _dataBaseContext.SaveChangesAsync();

                var messageText = "Успішне додавання...";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);

                var users = await _userService.GetUsersInRoleAsync(RoleNames.RepairEmployee);

                
                foreach (var user in users)
                {
                    await _botClient.SendTextMessageAsync(int.Parse(user.ChatId), $"Появився новий дефект в кімнаті: {defect.RoomNumber}", ParseMode.Markdown);
                }

                defect = new DTIS.Shared.Models.Defect();
            }
            catch (Exception ex)
            {
                var messageText = $"Помилка в FinishInputDefectState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
            }


        }
    }
}
