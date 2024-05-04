using UnityEngine;

public abstract class PersistentSingletonMonobehaviour<T> : SingletonMonoBehaviour<T> where T : Component
{
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
