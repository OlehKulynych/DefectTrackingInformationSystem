﻿using DefectTrackingInformationSystem.Service.Interfaces;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.Commands
{
    public class StartCommand: BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public StartCommand(IUserService userService, TelegramBotService telegramBotService)
        {
            _userService = userService;
            _botClient = telegramBotService.GetTelegramBot().Result;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreateUserAsync(update);
            var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Повідомити про дефект")
                                  

                }
            });

            await _botClient.SendTextMessageAsync(user.ChatId, "Це бот для того, щоб ви могли повідомити про знайдену вами поломку й техперсонал міг швидко її вирішити)",
                ParseMode.Markdown, replyMarkup: inlineKeyboard);
        }
    }
}

