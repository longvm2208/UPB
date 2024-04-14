#if UNITY_ANDROID || UNITY_IOS
using UnityEngine;
#endif
using UnityEngine.EventSystems;

public class PointerUtils
{
    public static bool IsOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 0) return false;

        Touch touch = Input.GetTouch(0);
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}
