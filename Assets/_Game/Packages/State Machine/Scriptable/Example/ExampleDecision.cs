using ScriptableStateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "ExampleDecision", menuName = "State Machine/ScriptableObjects/Decision/ExampleDecision")]
public class ExampleDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return false;
    }
}
