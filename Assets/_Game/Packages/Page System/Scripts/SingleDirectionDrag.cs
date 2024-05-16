using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SingleDirectionDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum Direction { None, Horizontal, Vertical }

    [SerializeField] private UnityEvent onBeginDrag;
    [SerializeField] private UnityEvent<Vector2> onDrag;
    [SerializeField] private UnityEvent onEndDrag;

    private bool isDragging;
    private Direction direction = Direction.None;
    private Vector2 previousPointerPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount >= 2)

        isDragging = true;
        previousPointerPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 delta = eventData.position - previousPointerPosition;

        if (delta.sqrMagnitude < 0.0001f) return;

        if (direction == Direction.None)
        {
            direction = delta.x > delta.y ? Direction.Horizontal : Direction.Vertical;
        }

        switch (direction)
        {
            case Direction.Horizontal:
                break;
            case Direction.Vertical:
                break;
            default:
                Debug.LogError("Invalid direction");
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        direction = Direction.None;
    }
}
