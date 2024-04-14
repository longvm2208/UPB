using UnityEngine;

namespace InterfaceStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private State1 state1;
        [SerializeField]
        private InterfaceState2 state2;

        private IBaseState<StateMachine> currentState;

        public State1 State1 => state1;
        public InterfaceState2 State2 => state2;

        private void Start()
        {
            SwitchState(state1);
        }

        private void Update()
        {
            currentState?.UpdateState(this);
        }

        public void SwitchState(IBaseState<StateMachine> nextState)
        {
            if (currentState != nextState)
            {
                currentState?.ExitState(this);
                currentState = nextState;
                currentState?.EnterState(this);
            }
        }
    }

}
