using UnityEngine;

public static class GameObjectExt
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T component)) return component;
        else return gameObject.AddComponent<T>();
    }
}
