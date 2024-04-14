using UnityEngine;

namespace ScriptableStateMachine
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateController controller);
    }
}
