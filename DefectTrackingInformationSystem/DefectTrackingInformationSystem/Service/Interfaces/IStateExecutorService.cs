using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.Service.Interfaces
{
    public interface IStateExecutorService
    {
        Task ExecuteStateAsync(Update update);
    }
}
