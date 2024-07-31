using UnityEngine;

/// <summary>
/// Provides a generic implementation for creating singleton MonoBehaviours.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name + " Auto-Generated");
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }

    public static bool HasInstance => instance != null;
}
