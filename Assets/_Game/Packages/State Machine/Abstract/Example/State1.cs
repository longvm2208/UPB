using UnityEngine;

namespace AbstractStateMachine.Example
{
    [System.Serializable]
    public class State1 : BaseState<StateMachine>
    {
        public override void Enter(StateMachine sm)
        {
            Debug.Log("Enter state 1");
        }

        public override void Update(StateMachine sm)
        {
            Debug.Log("Update state 1");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                sm.SwitchState(sm.state2);
            }
        }

        public override void Exit(StateMachine sm)
        {
            Debug.Log("Exit state 1");
        }
    }
}
