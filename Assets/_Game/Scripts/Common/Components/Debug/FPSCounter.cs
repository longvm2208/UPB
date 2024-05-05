using System.Collections;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private int size = 25;
    [SerializeField] private Rect rect = new Rect(5, 5, 200, 50);

    private float fps;
    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    private IEnumerator Start()
    {
        while (true)
        {
            fps = 1f / Time.unscaledDeltaTime;
            yield return wait;
        }
    }

    private void OnGUI()
    {
        GUI.depth = 2;
        GUI.color = Color.black;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = size;
        style.fontStyle = FontStyle.Bold;
        GUI.Label(rect, "FPS: " + Mathf.Round(fps), style);
    }
}
