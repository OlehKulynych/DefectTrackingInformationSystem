using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using DefectTrackingInformationSystem.Service.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.Commands
{
    public class AddNumberRoomCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataBaseContext _context;
        private readonly IUserService _userService;

        public AddNumberRoomCommand(TelegramBotService telegramBotService, DataBaseContext context, IUserService userService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;
            _context = context;
            _userService = userService;
        }

        public override string Name => CommandNames.InputNumberRooms;

        public override async Task ExecuteAsync(Update update)
        {
            var message = update.Message;
            if(message.Text != null)
            {
                uint numRoom;
                if(uint.TryParse(message.Text, out numRoom))
                {
                    var defect = new Defect { RoomNumber = numRoom };

                    await _context.Defectes.AddAsync(defect);
                    await _context.SaveChangesAsync();

                    var messageText = "Опишіть проблему: ";
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
                }
                else
                {
                    var messageText = "Повторіть ще раз, ви надіслали не число. ";
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
                }
            }
            else
            {

                var messageText = "Повторіть ще раз, тут має бути текст. ";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown);
            }
        }
    }
}
