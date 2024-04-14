using UnityEngine;

public static class ColliderExtensions
{
    public static T GetCachedComponent<T>(this Collider collider)
    {
        return ComponentByColliderCache<T>.Get(collider);
    }

    #region CENTER
    public static void ChangeCenterX(this BoxCollider boxCollider, float x)
    {
        boxCollider.center = new Vector3(x, boxCollider.center.y, boxCollider.center.z);
    }

    public static void ChangeCenterY(this BoxCollider boxCollider, float y)
    {
        boxCollider.center = new Vector3(boxCollider.center.x, y, boxCollider.center.z);
    }

    public static void ChangeCenterZ(this BoxCollider boxCollider, float z)
    {
        boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y, z);
    }
    #endregion

    #region SIZE
    public static void ChangeSizeX(this BoxCollider boxCollider, float x)
    {
        boxCollider.size = new Vector3(x, boxCollider.size.y, boxCollider.size.z);
    }

    public static void ChangeSizeY(this BoxCollider boxCollider, float y)
    {
        boxCollider.size = new Vector3(boxCollider.size.x, y, boxCollider.size.z);
    }

    public static void ChangeSizeZ(this BoxCollider boxCollider, float z)
    {
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, z);
    }
    #endregion
}

public static class Collider2DExtensions
{
    public static T GetCachedComponent<T>(this Collider2D collider2D)
    {
        return ComponentByCollider2DCache<T>.Get(collider2D);
    }
}
