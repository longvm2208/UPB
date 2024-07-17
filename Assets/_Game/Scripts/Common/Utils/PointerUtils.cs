using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerUtils
{
    public bool IsOverUI(int layer)
    {
        return IsOverUI(GetEventSystemRaycastResults(), layer);
    }

    bool IsOverUI(List<RaycastResult> results, int layer)
    {
        foreach (var result in results)
        {
            if (result.gameObject.layer == layer) return true;
        }

        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results;
    }
}
