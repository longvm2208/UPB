using UnityEngine;

namespace InterfaceStateMachine
{
    [System.Serializable]
    public class InterfaceState2 : IBaseState<StateMachine>
    {
        public void EnterState(StateMachine sm)
        {
            Debug.Log("Enter state 2");
        }

        public void UpdateState(StateMachine sm)
        {
            Debug.Log("Updating state 2");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                sm.SwitchState(sm.State1);
            }
        }

        public void ExitState(StateMachine sm)
        {
            Debug.Log("Exit state 2");
        }
    }
}
