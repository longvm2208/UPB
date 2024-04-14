using UnityEngine;

public class PrefabInstancePoolExample : MonoBehaviour
{
    private PrefabInstancePool<PrefabInstancePoolExample> pool;

    public PrefabInstancePoolExample Spawn()
    {
        PrefabInstancePoolExample instance = pool.GetInstance(this);
        instance.pool = pool;
        return instance;
    }

    public void Despawn()
    {
        pool.Recycle(this);
    }
}
