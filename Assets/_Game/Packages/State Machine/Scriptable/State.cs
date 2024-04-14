using UnityEngine;

namespace ScriptableStateMachine
{
    [CreateAssetMenu(fileName = "State", menuName = "State Machine/ScriptableObjects/State")]
    public class State : ScriptableObject
    {
        [SerializeField] private Action[] actions;
        [SerializeField] private Transition[] transitions;

        public void UpdateState(StateController controller)
        {
            DoActions(controller);
            CheckTransition(controller);
        }

        private void DoActions(StateController controller)
        {
            foreach (Action action in actions)
            {
                action.Act(controller);
            }
        }

        private void CheckTransition(StateController controller)
        {
            foreach (Transition transition in transitions)
            {
                bool result = transition.decision.Decide(controller);
                controller.TransitionToState(result ? transition.trueState : transition.falseState);
            }
        }
    }
}
