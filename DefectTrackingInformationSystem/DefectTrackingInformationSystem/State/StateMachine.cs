using System.Runtime.CompilerServices;
using Telegram.Bot.Types;

namespace DefectTrackingInformationSystem.State
{
    public class StateMachine
    {
        public State CurrentState { get; set; }
        public async Task Initialize(State startState, Update update)
        {
            CurrentState = startState;
            await CurrentState.ExecuteStateAsync(update);
        }

        public async Task ChangeState(State nextState, Update update)
        {

            CurrentState = nextState;
            await CurrentState.ExecuteStateAsync(update);
        }
    }
}
