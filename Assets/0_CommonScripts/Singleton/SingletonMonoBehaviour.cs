using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    public static T Ins { get; private set; }

    protected virtual void Awake()
    {
        if (Ins == null)
        {
            Ins = this as T;
        }
        else if (Ins != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (Ins == this)
        {
            Ins = null;
        }
    }
}
