using DefectTrackingInformationSystem.Commands;
using DefectTrackingInformationSystem.Service.Interfaces;
using DefectTrackingInformationSystem.State;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.Service
{
    public class StateExecutorService : IStateExecutorService
    {
        private StateMachine _stateMachine;
        private List<DefectTrackingInformationSystem.State.State> _states;
        private State.State _currentState;

        public StateExecutorService(IServiceProvider serviceProvider)
        {
            _states = serviceProvider.GetServices<DefectTrackingInformationSystem.State.State>().ToList();
            _stateMachine = new StateMachine();
            
        }

        public async Task ExecuteStateAsync(Update update)
        {
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;



            if (update.Type == UpdateType.Message)
            {

                if (update.Message != null && update.Message.Text.Contains(CommandNames.StartCommand))
                {
                    await ChangeStateAsync(CommandNames.StartCommand, update);
                    return;
                }

                switch (update.Message?.Text)
                {
                    case "Повідомити про дефект":
                        await ChangeStateAsync(CommandNames.InputDefectCommand, update);
                        return;
                }
            }

            switch (_currentState?.Name)
            {

                case CommandNames.InputDefectCommand:
                    {
                        await ChangeStateAsync(CommandNames.InputNumberRoomsCommand, update);
                        break;
                    }
                case CommandNames.InputNumberRoomsCommand:
                    {
                        await ChangeStateAsync(CommandNames.InputDecscriptionCommand, update);
                    }
                    break;
                case null:
                    {
                        await ChangeStateAsync(CommandNames.StartCommand, update);
                        break;
                    }
            }

        }

        private async Task ChangeStateAsync(string stateName, Update update)
        {
            _currentState = _states.First(s=> s.Name == stateName);
            await _currentState.ExecuteStateAsync(update);
        }

    }
}
