using DefectTrackingInformationSystem.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public abstract class BaseDefectState: BaseState
    {
        protected static DTIS.Shared.Models.Defect defect = new DTIS.Shared.Models.Defect();

    }
}
