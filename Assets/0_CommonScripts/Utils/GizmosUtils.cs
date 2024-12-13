using UnityEngine;

public class GizmosUtils
{
    public static void DrawTable(int width, int height, Vector2 center, Vector2 cellSize)
    {
        Vector2 root = new Vector2(-0.5f * width, -0.5f * height);

        for (int x = 0; x <= width; x++)
        {
            Vector2 start = root + new Vector2(x, 0);
            start = Vector2.Scale(start, cellSize) + center;

            Vector2 end = root + new Vector2(x, height);
            end = Vector2.Scale(end, cellSize) + center;

            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= height; y++)
        {
            Vector2 start = root + new Vector2(0, y);
            start = Vector2.Scale(start, cellSize) + center;

            Vector2 end = root + new Vector2(width, y);
            end = Vector2.Scale(end, cellSize) + center;

            Gizmos.DrawLine(start, end);
        }
    }

    public static void DrawPath(params Vector3[] path)
    {
        for (int i = 0; i < path.Length - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
