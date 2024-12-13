using UnityEditor;
using UnityEngine;

public class VectorUtils
{
    public static Vector2 WorldPositionToAnchorPosition(Vector3 worldPosition, RectTransform parent, Canvas canvas, Camera camera)
    {
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, canvas.worldCamera, out Vector2 localPoint);
        return localPoint;
    }

#if UNITY_EDITOR
    public static Vector3 ConvertEventToSceneViewPosition(Event eventGUI, SceneView sceneView)
    {
        Vector3 mousePosition = eventGUI.mousePosition;
        float pixels = EditorGUIUtility.pixelsPerPoint;
        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * pixels;
        mousePosition.x *= pixels;

        return mousePosition;
    }
#endif
}
