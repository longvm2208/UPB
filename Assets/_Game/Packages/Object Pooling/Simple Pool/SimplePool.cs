using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    private static int DEFAULT_AMOUNT = 10;
    private static Dictionary<PoolMember, Pool> poolOfWhichPrefab = new Dictionary<PoolMember, Pool>();
    private static Dictionary<PoolMember, Pool> memberOfWhichPool = new Dictionary<PoolMember, Pool>();

    public static void Preload(PoolMember prefab, int amount, Transform parent)
    {
        if (!poolOfWhichPrefab.ContainsKey(prefab))
        {
            poolOfWhichPrefab.Add(prefab, new Pool(prefab, amount, parent));
        }
    }

    #region SPAWN MEMBER
    public static PoolMember Spawn(PoolMember prefab)
    {
        if (!poolOfWhichPrefab.ContainsKey(prefab) || poolOfWhichPrefab[prefab] == null)
        {
            poolOfWhichPrefab.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, null));
        }

        PoolMember member = poolOfWhichPrefab[prefab].Spawn();

        return member;
    }

    public static PoolMember Spawn(PoolMember prefab, Transform parent)
    {
        if (!poolOfWhichPrefab.ContainsKey(prefab) || poolOfWhichPrefab[prefab] == null)
        {
            poolOfWhichPrefab.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, parent));
        }

        PoolMember member = poolOfWhichPrefab[prefab].Spawn(parent);

        return member;
    }

    public static PoolMember Spawn(PoolMember prefab, Transform parent, Vector3 localPosition)
    {
        if (!poolOfWhichPrefab.ContainsKey(prefab) || poolOfWhichPrefab[prefab] == null)
        {
            poolOfWhichPrefab.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, parent));
        }

        PoolMember member = poolOfWhichPrefab[prefab].Spawn(parent, localPosition);

        return member;
    }

    public static PoolMember Spawn(PoolMember prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolOfWhichPrefab.ContainsKey(prefab) || poolOfWhichPrefab[prefab] == null)
        {
            poolOfWhichPrefab.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, null));
        }

        PoolMember member = poolOfWhichPrefab[prefab].Spawn(position, rotation);

        return member;
    }
    #endregion

    #region SPAWN GENERIC
    public static T Spawn<T>(PoolMember prefab) where T : PoolMember
    {
        return Spawn(prefab) as T;
    }

    public static T Spawn<T>(PoolMember prefab, Transform parent) where T : PoolMember
    {
        return Spawn(prefab, parent) as T;
    }

    public static T Spawn<T>(PoolMember prefab, Transform parent, Vector3 localPosition) where T : PoolMember
    {
        return Spawn(prefab, parent, localPosition) as T;
    }

    public static T Spawn<T>(PoolMember prefab, Vector3 position, Quaternion rotation) where T : PoolMember
    {
        return Spawn(prefab, position, rotation) as T;
    }
    #endregion

    public static void Despawn(PoolMember member)
    {
        if (memberOfWhichPool.ContainsKey(member))
        {
            memberOfWhichPool[member].Despawn(member);
        }
        else
        {
            Object.Destroy(member);
        }
    }

    public static void CollectAll()
    {
        foreach (var item in poolOfWhichPrefab)
        {
            item.Value.Collect();
        }
    }

    public static void ReleaseAll()
    {
        foreach (var item in poolOfWhichPrefab)
        {
            item.Value.Release();
        }
    }

    public class Pool
    {
        private Transform parent;
        private PoolMember prefab;
        private Queue<PoolMember> pool = new Queue<PoolMember>();
        private List<PoolMember> active = new List<PoolMember>();

        public Pool(PoolMember prefab, int amount, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < amount; i++)
            {
                PoolMember member = Object.Instantiate(prefab, parent);
                memberOfWhichPool.Add(member, this);
                pool.Enqueue(member);
                member.gameObject.SetActive(false);
            }
        }

        #region SPAWN
        public PoolMember Spawn()
        {
            PoolMember member;

            if (pool.Count == 0)
            {
                member = Object.Instantiate(prefab, parent);
                memberOfWhichPool.Add(member, this);
            }
            else
            {
                member = pool.Dequeue();
            }

            member.gameObject.SetActive(true);

            active.Add(member);

            return member;
        }

        public PoolMember Spawn(Transform parent)
        {
            PoolMember member;

            if (pool.Count == 0)
            {
                member = Object.Instantiate(prefab, parent);
                memberOfWhichPool.Add(member, this);
            }
            else
            {
                member = pool.Dequeue();
            }

            member.gameObject.SetActive(true);

            active.Add(member);

            return member;
        }

        public PoolMember Spawn(Transform parent, Vector3 localPosition)
        {
            PoolMember member;

            if (pool.Count == 0)
            {
                member = Object.Instantiate(prefab, parent);
                memberOfWhichPool.Add(member, this);
            }
            else
            {
                member = pool.Dequeue();
            }

            member.transform.localPosition = localPosition;
            member.gameObject.SetActive(true);

            active.Add(member);

            return member;
        }

        public PoolMember Spawn(Vector3 position, Quaternion rotation)
        {
            PoolMember member;

            if (pool.Count == 0)
            {
                member = Object.Instantiate(prefab, parent);
                memberOfWhichPool.Add(member, this);
            }
            else
            {
                member = pool.Dequeue();
            }

            member.transform.SetPositionAndRotation(position, rotation);
            member.gameObject.SetActive(true);

            active.Add(member);

            return member;
        }
        #endregion

        public void Despawn(PoolMember member)
        {
            active.Remove(member);
            pool.Enqueue(member);
            member.gameObject.SetActive(false);
        }

        public void Collect()
        {
            while (active.Count > 0)
            {
                Despawn(active[0]);
            }
        }

        public void Release()
        {
            Collect();

            while (pool.Count > 0)
            {
                PoolMember member = pool.Dequeue();
                Object.Destroy(member);
            }
        }
    }
}

public class PoolMember : MonoBehaviour { }
