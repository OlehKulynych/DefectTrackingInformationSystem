using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using DefectTrackingInformationSystem.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUserService _userService;
        public FixDefectState(TelegramBotService telegramBotService, DataBaseContext dataBaseContext, IUserService userService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _dataBaseContext = dataBaseContext;
            _userService = userService;
        }

        public override string Name => CommandNames.FixDefectCommand;

        public override async Task ExecuteStateAsync(Update update)
        {

            try
            {
                var user = await _userService.GetUserAsync(update);
                if(user != null)
                {
                    if(await _userService.IsInRoleAsync(user, RoleNames.RepairEmployee))
                    {
                        StringBuilder message = new StringBuilder();
                        message.AppendLine("Невирішені дефекти: \n");

                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
                        message.Clear();

                        var defects = _dataBaseContext.Defectes.Where(x => x.isClosed == false).ToList();
                        if (defects.Count() != 0)
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
                            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Немає наявних дефектів, можна відпочити)))", ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
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
            catch(Exception ex)
            {
                var messageText = $"Помилка в FixDefectState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
            }

            
           

        }

        private IReplyMarkup GetInlineButton(int Id)
        {
            return new InlineKeyboardMarkup( new InlineKeyboardButton("Fix") { CallbackData = Id.ToString()});
        }
    }
}
