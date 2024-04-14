using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListenerGeneric<T, E, U> : MonoBehaviour, IGameEventListenerGeneric<T> where E : GameEventGeneric<T> where U : UnityEvent<T>
{
    [SerializeField] private E gameEvent;
    [SerializeField] private U response;

    private void OnEnable()
    {
        if (gameEvent == null)
        {
            return;
        }

        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
        {
            return;
        }

        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T t)
    {
        response?.Invoke(t);
    }
}
