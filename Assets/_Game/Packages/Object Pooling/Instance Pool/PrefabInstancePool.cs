using System.Collections.Generic;
using UnityEngine;

public struct PrefabInstancePool<T> where T : MonoBehaviour
{
    private Stack<T> pool;

    public T GetInstance(T prefab)
    {
        if (pool == null)
        {
            pool = new Stack<T>();
        }
#if UNITY_EDITOR
        else if (pool.Count != 0 && pool.Peek() == null)
        {
            // Instances destroyed, assuming due to exiting play mode.
            pool.Clear();
        }
#endif
        T instance;
        if (pool.Count > 0)
        {
            instance = pool.Pop();
            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = Object.Instantiate(prefab);
        }

        return instance;
    }

    public void Recycle(T instance)
    {
#if UNITY_EDITOR
        if (pool == null)
        {
            // Pool lost, assuming due to hot reload.
            Object.Destroy(instance.gameObject);
            return;
        }
#endif
        if (!pool.Contains(instance))
        {
            pool.Push(instance);
            instance.gameObject.SetActive(false);
        }
    }
}
