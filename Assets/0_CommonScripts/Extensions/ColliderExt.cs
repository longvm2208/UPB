using UnityEngine;

public static class ColliderExt
{
    #region BOX COLLIDER
    public static void ChangeCenterX(this BoxCollider box, float x)
    {
        box.center = new Vector3(x, box.center.y, box.center.z);
    }

    public static void ChangeCenterY(this BoxCollider box, float y)
    {
        box.center = new Vector3(box.center.x, y, box.center.z);
    }

    public static void ChangeCenterZ(this BoxCollider box, float z)
    {
        box.center = new Vector3(box.center.x, box.center.y, z);
    }

    public static void ChangeSizeX(this BoxCollider box, float x)
    {
        box.size = new Vector3(x, box.size.y, box.size.z);
    }

    public static void ChangeSizeY(this BoxCollider box, float y)
    {
        box.size = new Vector3(box.size.x, y, box.size.z);
    }

    public static void ChangeSizeZ(this BoxCollider box, float z)
    {
        box.size = new Vector3(box.size.x, box.size.y, z);
    }

    public static void ChangeOffsetX(this BoxCollider2D box, float x)
    {
        box.offset = new Vector2(x, box.offset.y);
    }

    public static void ChangeOffsetY(this BoxCollider2D box, float y)
    {
        box.offset = new Vector2(box.offset.x, y);
    }

    public static void ChangeSizeX(this BoxCollider2D box, float x)
    {
        box.size = new Vector2(x, box.size.y);
    }

    public static void ChangeSizeY(this BoxCollider2D box, float y)
    {
        box.size = new Vector2(box.size.x, y);
    }
    #endregion
}
