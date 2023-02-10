using DefectTrackingInformationSystem.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public abstract class State
    {
        protected static Defect defect = new Defect();
        public abstract string Name { get; }
        public abstract Task ExecuteStateAsync(Update update);

    }
}
