using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.IO;

namespace DefectTrackingInformationSystem.State
{
    public class GetDefectsState : BaseState
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;

        public GetDefectsState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
        }

        public override string Name => CommandNames.GetDefectsCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Дефекти: \n");
            var defects = _dataBaseContext.Defectes.ToList();

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
            message.Clear();


            foreach (var el in defects)
            {

                message.AppendLine($" ----- \n Id: {el.Id} \n Номер кімнати: {el.RoomNumber} \n Опис: {el.Description}");
                if (el.ImageString != null)
                {

                    var directory = new DirectoryInfo(
                    Directory.GetCurrentDirectory());

                    var imagePath = Path.Combine(directory.Parent.FullName, $"{el.ImageString}");

                    using (var stream = System.IO.File.OpenRead(imagePath))
                    {

                        await _botClient.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream), caption: message.ToString(), ParseMode.Markdown);
                    }

                        //var bytes = Convert.FromBase64String(el.ImageString);
                    //var imageStream = new MemoryStream(bytes);

                }
                else
                {

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
                }
                message.Clear();

            }

        }
    }
}
