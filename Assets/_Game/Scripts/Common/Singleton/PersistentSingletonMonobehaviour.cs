using UnityEngine;

public abstract class PersistentSingletonMonobehaviour<T> : MonoBehaviour where T : Component
{
    static T instance;

    public static T Instance => instance;

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
