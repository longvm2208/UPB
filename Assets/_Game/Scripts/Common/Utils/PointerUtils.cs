using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerUtils
{
    public bool IsPointerOverUIElement(int UILayer)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), UILayer);
    }

    private bool IsPointerOverUIElement(List<RaycastResult> results, int UILayer)
    {
        foreach (var result in results)
        {
            if (result.gameObject.layer == UILayer) return true;
        }

        return false;
    }

    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

}
