using System.Collections;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [SerializeField] private float elasticity = 0.1f;
    [SerializeField] private float modifier = 1f;
    [SerializeField] private float smoothDuration = 0.5f;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform root;

    private Vector2 velocity;
    private IEnumerator smoothMoveCoroutine;

    public void OnBeginDrag()
    {
        StopSmoothMove();

        velocity = Vector2.zero;
    }

    public void OnDrag(Vector2 delta)
    {
        delta.x = 0f;
        content.anchoredPosition += modifier * delta;
        velocity = delta / Time.deltaTime;
    }

    public void OnEndDrag()
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
            destination += velocity.y * elasticity;
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

