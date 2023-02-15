using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service.Interfaces;
using DefectTrackingInformationSystem.Service;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public class InputNumberRoomState: BaseDefectState
    {
        private readonly TelegramBotClient _botClient;
    
       

        public InputNumberRoomState(TelegramBotService telegramBotService)
        {
            _botClient = telegramBotService.GetTelegramBot().Result;  
            
        }

        public override string Name => CommandNames.InputNumberRoomsCommand;

        public override async Task ExecuteStateAsync(Update update)
        {
            try
            {
                var message = update.Message;
                if (message.Text != null)
                {
                    uint numRoom;
                    if (uint.TryParse(message.Text, out numRoom))
                    {
                        defect.RoomNumber = numRoom;

                        var messageText = "Опишіть проблему: ";
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
                    }
                    else
                    {
                        var messageText = "Повторіть ще раз, ви надіслали не число. ";
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
                    }
                }
                else
                {
                    var messageText = "Повторіть ще раз, тут має бути текст. ";
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
                }
            }
            catch(Exception ex)
            {
                var messageText = $"Помилка в InputNumberRoomState: \n{ex.Message}";
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, messageText, ParseMode.Markdown, replyMarkup: Keyboards.GetButtons());
            }           
        }
    }
}
