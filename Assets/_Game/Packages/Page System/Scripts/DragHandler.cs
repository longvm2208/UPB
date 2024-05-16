using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Direction { None, Horizontal, Vertical }

    [SerializeField] private UnityEvent onBeginDrag;
    [SerializeField] private UnityEvent<Vector2> onDrag;
    [SerializeField] private UnityEvent onEndDrag;

    private bool isDragging;
    private bool isEnabled = true;
    private Direction direction = Direction.None;
    private Vector2 previousPointerPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount >= 2 || !isEnabled) return;

        isDragging = true;
        previousPointerPosition = eventData.position;

        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || !isEnabled) return;

        Vector2 delta = eventData.position - previousPointerPosition;
        previousPointerPosition = eventData.position;

        if (delta.sqrMagnitude < 0.0001f) return;

        if (direction == Direction.None)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                direction = Direction.Horizontal;
            }
            else
            {
                direction = Direction.Vertical;
            }
        }

        if (direction == Direction.Horizontal)
        {
            delta.y = 0f;
        }
        else
        {
            delta.x = 0f;
        }

        onDrag?.Invoke(delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging || !isEnabled) return;

        isDragging = false;
        direction = Direction.None;

        onEndDrag?.Invoke();
    }

    public void Enable() => isEnabled = true;
    public void Disable() => isEnabled = false;
}
