using UnityEngine;

namespace AbstractStateMachine
{
    public abstract class BaseState<T> where T : MonoBehaviour
    {
        public abstract void Enter(T t);
        public abstract void Update(T t);
        public abstract void Exit(T t);
    }
}
