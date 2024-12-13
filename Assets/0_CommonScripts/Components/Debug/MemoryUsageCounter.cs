using System.Collections;
using UnityEngine;

public class MemoryUsageCounter : MonoBehaviour
{
    [SerializeField] private int size = 25;
    [SerializeField] private Rect rect = new Rect(5, 35, 200, 50);

    private float memoryUsage;
    private WaitForSeconds wait = new WaitForSeconds(1f);

    private IEnumerator Start()
    {
        while (true)
        {
            memoryUsage = (float)System.GC.GetTotalMemory(false) / (1024 * 1024);
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
        GUI.Label(rect, "RAM: " + memoryUsage.ToString("F1") + " MB", style);
    }
}
