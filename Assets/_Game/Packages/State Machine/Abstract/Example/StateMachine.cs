using UnityEngine;

namespace AbstractStateMachine.Example
{
    public class StateMachine : MonoBehaviour
    {
        public State1 state1;
        public State2 state2;

        private BaseState<StateMachine> currentState;

        private void Start()
        {
            SwitchState(state1);
        }

        private void Update()
        {
            currentState?.Update(this);
        }

        public void SwitchState(BaseState<StateMachine> nextState)
        {
            if (currentState != nextState)
            {
                currentState?.Exit(this);
                currentState = nextState;
                currentState?.Enter(this);
            }
        }
    }
}
