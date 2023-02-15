using DefectTrackingInformationSystem.Constants;
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
using DefectTrackingInformationSystem.Service.Interfaces;

namespace DefectTrackingInformationSystem.State
{
    public class GetDefectsState : BaseState
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _dataBaseContext;
        private readonly IUserService _userService;

        public GetDefectsState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext, IUserService userService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
            _userService = userService;
        }

        public override string Name => CommandNames.GetDefectsCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            try
            {
                var user = await _userService.GetUserAsync(update);
                if (user != null)
                {
                    if (await _userService.IsInRoleAsync(user, RoleNames.RepairEmployee))
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
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
                            }
                            message.Clear();
                        }
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ахх ти хитрун, в тебе доступу немає, більше такого не роби...", ParseMode.Markdown);
                    }
                }
                else
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ти взагалі хто?", ParseMode.Markdown);
                }
            }
            catch (Exception ex)
            {
                var messageText = $"Помилка в GetDefectsState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
            }
                        
        }
    }
}
