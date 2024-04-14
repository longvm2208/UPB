using System;
using System.Collections.Generic;

/// <summary>
/// Provides functionality to manage event listeners and dispatch events.
/// </summary>
public class EventDispatcher
{
    private static Dictionary<EventId, Action<object>> eventListeners = new();

    /// <summary>
    /// Registers a listener for a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to register the listener for.</param>
    /// <param name="callback">The callback method to be invoked when the event occurs.</param>
    public static void RegisterListener(EventId eventId, Action<object> callback)
    {
        if (!eventListeners.ContainsKey(eventId))
        {
            eventListeners.Add(eventId, null);
        }

        eventListeners[eventId] += callback;
    }

    /// <summary>
    /// Unregisters a listener from a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to unregister the listener from.</param>
    /// <param name="callback">The callback method to be unregistered.</param>
    public static void UnregisterListener(EventId eventId, Action<object> callback)
    {
        if (eventListeners.ContainsKey(eventId))
        {
            eventListeners[eventId] -= callback;
        }
    }

    /// <summary>
    /// Dispatches a specified event with an optional parameter.
    /// </summary>
    /// <param name="eventId">The ID of the event to dispatch.</param>
    /// <param name="parameter">Optional parameter to be passed to the event listeners.</param>
    public static void DispatchEvent(EventId eventId, object parameter = null)
    {
        if (!eventListeners.ContainsKey(eventId))
        {
            return;
        }

        if (eventListeners[eventId] == null)
        {
            eventListeners.Remove(eventId);
        }

        eventListeners[eventId].Invoke(parameter);
    }

    /// <summary>
    /// Clears all registered listeners for all events.
    /// </summary>
    public void ClearAllListeners()
    {
        eventListeners.Clear();
    }
}
