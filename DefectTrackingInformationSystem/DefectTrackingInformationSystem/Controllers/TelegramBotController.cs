using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = DefectTrackingInformationSystem.Models.User;

namespace DefectTrackingInformationSystem.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : Controller
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly DataBaseContext dataBaseContext;
        public TelegramBotController(TelegramBotService telegramBot, DataBaseContext dataBaseContext)
        {
            _telegramBotClient = telegramBot.GetTelegramBot().Result;
            this.dataBaseContext = dataBaseContext;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]object update)
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
            var chat = upd.Message?.Chat;
            var user = new User
            {
                ChatId = chat.Id.ToString(),
                FirstName = chat.FirstName,
                LastName = chat.LastName,
                Id = Guid.NewGuid().ToString()


            };

            var result = await dataBaseContext.Users.AddAsync(user);
            await dataBaseContext.SaveChangesAsync();
            await _telegramBotClient.SendTextMessageAsync(chat.Id, "Register",ParseMode.Markdown);
            return Ok();
        }
    }
}
