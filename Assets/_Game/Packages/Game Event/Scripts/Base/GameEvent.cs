using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Event/Game Event")]
public class GameEvent : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea()]
    [Tooltip("For goldfish brain")]
    private string description;
#endif

    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            return;
        }

        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            return;
        }

        listeners.Remove(listener);
    }
}
