using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public abstract class BaseState
    {
        public abstract string Name { get; }
        public abstract Task ExecuteStateAsync(Update update);
    }

}
