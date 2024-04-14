using UnityEditor;
using UnityEngine;

public class VectorUtils
{
    public static Vector2 WorldToCanvasCameraExpand(Vector3 position, float baseWidth = 1080f, float baseHeight = 1920f)
    {
        float matchX = baseWidth;
        float matchY = baseHeight;
        float baseRatio = baseWidth / baseHeight;
        float screenRatio = (float)Screen.width / Screen.height;

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);

        if (screenRatio < baseRatio)
        {
            matchX = baseWidth;
            matchY = matchX / screenRatio;
        }
        else if (screenRatio > baseRatio)
        {
            matchY = baseHeight;
            matchX = matchY * screenRatio;
        }

        float convertRatio = matchX / Screen.width;

        return screenPoint * convertRatio - new Vector2(matchX, matchY) * 0.5f;
    }

    public static Vector3 CanvasCameraToWorld(Vector2 canvasPos, Camera camera = null)
    {
        if (camera == null) camera = Camera.main;

        Vector3 screenPos = (Vector3)canvasPos + Vector3.back * 5f;
        Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);

        return worldPos;
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
