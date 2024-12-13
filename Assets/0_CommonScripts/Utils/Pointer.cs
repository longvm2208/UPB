using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer
{
    static EventSystem eventSystem;
    static PointerEventData pointerEventData;

    public static bool IsOverLayer(int layer)
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
            pointerEventData = new PointerEventData(eventSystem);
        }

        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new();
        eventSystem.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.layer == layer) return true;
        }

        return false;
    }
}
