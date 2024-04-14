using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventGeneric<T> : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea()]
    [Tooltip("For goldfish brain")]
    private string description;
#endif

    private readonly List<IGameEventListenerGeneric<T>> listeners = new List<IGameEventListenerGeneric<T>>();

    public void Raise(T t)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(t);
        }
    }

    public void RegisterListener(IGameEventListenerGeneric<T> listener)
    {
        if (listeners.Contains(listener))
        {
            return;
        }

        listeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListenerGeneric<T> listener)
    {
        if (!listeners.Contains(listener))
        {
            return;
        }

        listeners.Remove(listener);
    }
}
