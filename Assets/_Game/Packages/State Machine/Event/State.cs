using System;

namespace EventStateMachine
{
    [Serializable]
    public class State
    {
        public delegate void StateAction(ref Action onEnter, ref Action onUpdate, ref Action onExit);
        private Action onEnter, onUpdate, onExit;

        public void Update()
        {
            onUpdate?.Invoke();
        }

        public void ChangeState(StateAction stateAction)
        {
            onExit?.Invoke();
            stateAction.Invoke(ref onEnter, ref onUpdate, ref onExit);
            onEnter?.Invoke();
        }
    }
}
