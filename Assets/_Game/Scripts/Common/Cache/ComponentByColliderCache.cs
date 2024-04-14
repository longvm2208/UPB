using System.Collections.Generic;
using UnityEngine;

public class ComponentByColliderCache<T>
{
    private static Dictionary<Collider, T> componentByCollider;

    public static T Get(Collider collider)
    {
        if (componentByCollider == null)
        {
            componentByCollider = new Dictionary<Collider, T>();
        }

        if (!componentByCollider.ContainsKey(collider))
        {
            componentByCollider.Add(collider, collider.GetComponent<T>());
        }

        return componentByCollider[collider];
    }

    public static void ClearCache()
    {
        if (componentByCollider != null)
        {
            componentByCollider.Clear();
        }
    }
}

public class ComponentByCollider2DCache<T>
{
    private static Dictionary<Collider2D, T> componentByCollider2D;

    public static T Get(Collider2D collider2D)
    {
        if (componentByCollider2D == null)
        {
            componentByCollider2D = new Dictionary<Collider2D, T>();
        }

        if (!componentByCollider2D.ContainsKey(collider2D))
        {
            componentByCollider2D.Add(collider2D, collider2D.GetComponent<T>());
        }

        return componentByCollider2D[collider2D];
    }

    public static void ClearCache()
    {
        if (componentByCollider2D != null)
        {
            componentByCollider2D.Clear();
        }
    }
}
