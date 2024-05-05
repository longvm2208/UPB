using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera myCamera;
    [SerializeField, Min(0f)] private float min = 1f;
    [SerializeField, Min(0f)] private float max = 8f;
    [SerializeField, Min(0f)] private float modifier = 0.01f;

    private void Update()
    {
        Zooming();
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }

    private void Zooming()
    {
        if (Application.isEditor)
        {
            ZoomingPc();
        }
        else
        {
            ZoomingMobile();
        }
    }

    private void ZoomingPc()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f))
        {
            return;
        }
        Zoom(scroll);
    }

    private void ZoomingMobile()
    {
        if (Input.touchCount != 2) return;

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        Vector2 previousPosition0 = touch0.position - touch0.deltaPosition;
        Vector2 previousPosition1 = touch1.position - touch1.deltaPosition;

        float previousDistance = Vector2.Distance(previousPosition0, previousPosition1);
        float currentDistance = Vector2.Distance(touch0.position, touch1.position);
        float difference = currentDistance - previousDistance;

        if (Mathf.Approximately(difference, 0f))
        {
            return;
        }

        Zoom(difference * modifier);
    }

    private void Zoom(float difference)
    {
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - difference, min, max);
    }
}
