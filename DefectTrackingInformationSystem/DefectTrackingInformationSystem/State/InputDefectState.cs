﻿using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using DefectTrackingInformationSystem.Service.Interfaces;

namespace DefectTrackingInformationSystem.State
{
    public class InputDefectState: BaseDefectState
    {
        private readonly TelegramBotClient _botClient;
        private readonly IUserService _userService;

        

        public override string Name => CommandNames.InputDefectCommand;

        public InputDefectState(TelegramBotService telegramBotService, IUserService userService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _userService = userService;
        }
      
        public override async Task ExecuteStateAsync(Update update)
        {
            try
            {
                var user = await _userService.GetUserAsync(update);
                if (user != null)
                {
                    if (await _userService.IsInRoleAsync(user, RoleNames.TechnicalStaff))
                    {
                        const string message = "Введіть номер кімнати з дефектом (число): ";
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ахх ти хитрун, в тебе доступу немає, більше такого не роби...", ParseMode.Markdown);
                    }
                }
                else
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Ти хто такий?", ParseMode.Markdown);
                }
            }
            catch(Exception ex)
            {
                var messageText = $"Помилка в InputDefectState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
            }
        }
    }
}
