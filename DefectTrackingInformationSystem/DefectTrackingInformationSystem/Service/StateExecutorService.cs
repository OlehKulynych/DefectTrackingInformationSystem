using DefectTrackingInformationSystem.Constants;
using DefectTrackingInformationSystem.Service.Interfaces;
using DefectTrackingInformationSystem.State;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DefectTrackingInformationSystem.Service
{
    public class StateExecutorService : IStateExecutorService
    {
        private List<BaseState> _states;
        private BaseState _currentState;

        public StateExecutorService(IServiceProvider serviceProvider)
        {
            _states = serviceProvider.GetServices<BaseState>().ToList();
            _states.AddRange(serviceProvider.GetServices<BaseDefectState>().ToList());
            
        }

        public async Task ExecuteStateAsync(Update update)
        {
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;



            if (update.Type == UpdateType.Message)
            {
                if(update.Message.Text != null)
                {
                    if (update.Message != null && update.Message.Text.Contains(CommandNames.StartCommand))
                    {
                        await ChangeStateAsync(CommandNames.StartCommand, update);
                        return;
                    }

                    switch (update.Message?.Text)
                    {
                        case "Повідомити про дефект":
                            {
                                await ChangeStateAsync(CommandNames.InputDefectCommand, update);
                                return;
                            }
                        case "Переглянути наявні дефекти":
                            {
                                await ChangeStateAsync(CommandNames.GetDefectsCommand, update);
                                return;
                            }
                        case "Виправити дефекти":
                            {
                                await ChangeStateAsync(CommandNames.FixDefectCommand, update);
                                return;
                            }
                        case "Завантажити фото дефекту":
                            {
                                if (_currentState?.Name == CommandNames.InputDecscriptionCommand)
                                {

                                    await ChangeStateAsync(CommandNames.StartInputImageDefectCommand, update);
                                }
                                return;
                            }
                        case "Завершити":
                            {
                                if(_currentState?.Name == CommandNames.InputDecscriptionCommand || _currentState?.Name == CommandNames.InputImageDefectCommand)
                                {

                                    await ChangeStateAsync(CommandNames.FinishInputDefectCommand, update);
                                }
                                return;
                            }
                        case "Перейти в меню":
                            {
                                await ChangeStateAsync(CommandNames.StartCommand, update);
                                return;
                            }
                    }
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
                case CommandNames.StartInputImageDefectCommand:
                    {
                        await ChangeStateAsync(CommandNames.InputImageDefectCommand, update);
                    }break;
                case CommandNames.InputImageDefectCommand:
                    {
                        await ChangeStateAsync(CommandNames.FinishInputDefectCommand, update);
                    }
                    break;

                case CommandNames.FixDefectCommand:
                    {
                        await ChangeStateAsync(CommandNames.FinishFixDefectCommand, update);
                    }
                    break;
                case CommandNames.FinishFixDefectCommand:
                    {
                        await ChangeStateAsync(CommandNames.FinishFixDefectCommand, update);
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
