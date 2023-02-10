using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.Service.Interfaces
{
    public interface ICommandExecutorService

    {
        Task ExecuteAsync(Update update);
    }
}
