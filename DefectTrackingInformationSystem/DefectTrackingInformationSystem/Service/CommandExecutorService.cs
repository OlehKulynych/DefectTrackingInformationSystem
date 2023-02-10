using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Models;
using DefectTrackingInformationSystem.Service.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.Service
{
    public class CommandExecutorService : ICommandExecutorService
    {
        private readonly List<BaseCommand> _commands;
        private BaseCommand _lastCommand;
        

        public CommandExecutorService(IServiceProvider serviceProvider)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        public async Task ExecuteAsync(Update update)
        {
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message?.Text)
                {
                    case "Повідомити про дефект":
                        await ExecuteCommand(CommandNames.AddDefectCommand, update);
                        return;
                }
            }

           

            if (update.Message != null && update.Message.Text.Contains(CommandNames.StartCommand))
            {
                await ExecuteCommand(CommandNames.StartCommand, update);
                return;
            }
          
            switch (_lastCommand?.Name)
            {
               
                case CommandNames.AddDefectCommand:
                    {
                        await ExecuteCommand(CommandNames.InputNumberRooms, update);
                        break;
                    }
                case CommandNames.InputNumberRooms:
                    {
                        await ExecuteCommand(CommandNames.FinishOperationCommand, update);
                    }
                    break;
                case null:
                    {
                        await ExecuteCommand(CommandNames.StartCommand, update);
                        break;
                    }
            }
          
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update);
        }
    }
}
