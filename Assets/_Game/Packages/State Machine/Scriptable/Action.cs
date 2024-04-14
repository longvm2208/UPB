using UnityEngine;

namespace ScriptableStateMachine
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}
