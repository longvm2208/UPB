using UnityEngine;

namespace InterfaceStateMachine
{
    [System.Serializable]
    public class State1 : IBaseState<StateMachine>
    {
        public void EnterState(StateMachine sm)
        {
            Debug.Log("Enter state 1");
        }

        public void UpdateState(StateMachine sm)
        {
            Debug.Log("Updating state 1");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                sm.SwitchState(sm.State2);
            }
        }

        public void ExitState(StateMachine sm)
        {
            Debug.Log("Exit state 1");
        }
    }
}
