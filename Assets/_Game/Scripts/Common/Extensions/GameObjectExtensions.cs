using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Gets a component of the given type attached to the GameObject. If that type of component does not exist, it adds one.
    /// </summary>
    /// <remarks>
    /// This method is useful when you don't know if a GameObject has a specific type of component,
    /// but you want to work with that component regardless. Instead of checking and adding the component manually,
    /// you can use this method to do both operations in one line.
    /// </remarks>
    /// <typeparam name="T">The type of the component to get or add.</typeparam>
    /// <param name="gameObject">The GameObject to get the component from or add the component to.</param>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T component)) return component;
        else return gameObject.AddComponent<T>();
    }
}
