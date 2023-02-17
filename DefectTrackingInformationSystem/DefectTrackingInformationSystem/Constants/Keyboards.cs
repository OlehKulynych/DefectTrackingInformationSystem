using Telegram.Bot.Types.ReplyMarkups;

namespace DefectTrackingInformationSystem.Constants
{
    public class Keyboards
    {
        public static IReplyMarkup GetButtons()
        {
            List<List<KeyboardButton>> keyboardButtons = new List<List<KeyboardButton>>();

            keyboardButtons.Add(new List<KeyboardButton> { new KeyboardButton("Перейти в меню") });
           
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            return replyKeyboardMarkup;
        }
    }
}
