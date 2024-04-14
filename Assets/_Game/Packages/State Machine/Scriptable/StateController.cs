using UnityEngine;

namespace ScriptableStateMachine
{
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] protected State currentState;
        [SerializeField] protected State remainState;

        protected virtual void Update()
        {
            currentState.UpdateState(this);
        }

        public virtual void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
            }
        }
    }
}
