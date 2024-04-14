namespace InterfaceStateMachine
{
    public interface IBaseState<T>
    {
        void EnterState(T t);
        void UpdateState(T t);
        void ExitState(T t);
    }
}
