using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service;
using DefectTrackingInformationSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
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
        
        private readonly IStateExecutorService _stateExecutorService;

        public TelegramBotController(IStateExecutorService stateExecutorService)
        {
            _stateExecutorService = stateExecutorService;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]object update)
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

            if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
            {
                return Ok();
            }

            try
            {
                await _stateExecutorService.ExecuteStateAsync(upd);
            }
            catch (Exception e)
            {
                
                return Ok();
            }

            return Ok();
        }
    }
}
