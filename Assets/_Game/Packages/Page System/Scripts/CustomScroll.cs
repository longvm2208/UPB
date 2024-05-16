using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomScroll : MonoBehaviour
{
    [SerializeField] private float elasticity = 0.1f;
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float smoothDuration = 0.5f;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform root;

    private Vector2 dragStartPosition;
    private Vector2 dragPreviousPosition;
    private Vector2 contentStartPosition;
    private Vector2 contentStartDragPosition;
    private Vector2 different;
    private float velocity;
    private IEnumerator smoothMoveCoroutine;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
        dragPreviousPosition = dragStartPosition;
        contentStartPosition = content.anchoredPosition;
        contentStartDragPosition = contentStartPosition;
        different = Vector2.zero;
        velocity = 0f;
        StopSmoothMove();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - dragStartPosition;
        content.ChangeAnchorPosY(contentStartDragPosition.y + delta.y * scrollSpeed);
        velocity = (eventData.position.y - dragPreviousPosition.y) / Time.deltaTime;
        dragPreviousPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartSmoothMove();
    }

    private void StartSmoothMove()
    {
        float destination = content.anchoredPosition.y;
        float maxY = content.rect.height * content.localScale.x - root.rect.height;

        if (content.anchoredPosition.y < 0)
        {
            destination = 0f;
        }
        else if (content.anchoredPosition.y > maxY)
        {
            destination = maxY;
        }
        else
        {
            destination += velocity * elasticity;
            destination = Mathf.Clamp(destination, 0f, maxY);
        }

        smoothMoveCoroutine = Routine();
        StartCoroutine(smoothMoveCoroutine);

        IEnumerator Routine()
        {
            float t = 0f;
            float start = content.anchoredPosition.y;

            while (t <= 1f)
            {
                t += Time.deltaTime / smoothDuration;
                content.ChangeAnchorPosY(Mathf.Lerp(start, destination, Mathf.SmoothStep(0f, 1f, t)));
                yield return null;
            }
        }
    }

    private void StopSmoothMove()
    {
        if (smoothMoveCoroutine != null)
        {
            StopCoroutine(smoothMoveCoroutine);
        }
    }
}

