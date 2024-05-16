using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private bool canExceed = true;
    [SerializeField] private int startIndex = 1;
    [SerializeField] private float smoothMoveDuration = 0.5f;
    [SerializeField] private float percentThreshold = 0.2f;
    [SerializeField] private RectTransform root;
    [SerializeField] private GameObject[] pages;
    [SerializeField] private PageButton[] pageButtons;
    [SerializeField] private UnityEvent<PointerEventData> onBeginDrag;
    [SerializeField] private UnityEvent<PointerEventData> onDrag;
    [SerializeField] private UnityEvent<PointerEventData> onEndDrag;

    private bool isDragging;
    private bool isEnabled = true;
    private bool isCulling = true;
    private int currentIndex;
    private int minIndex;
    private int maxIndex;
    private Vector3 currentRootPosition;
    private Vector2 previousDragPosition;
    private Vector2 different;
    private IEnumerator smoothMoveCoroutine;

    private void Start() => Initialize();

    public void Initialize()
    {
        currentIndex = startIndex;
        minIndex = 0;
        maxIndex = pages.Length - 3;
        currentRootPosition = root.position;
        EnableCulling();

        for (int i = 0; i < pageButtons.Length; i++)
        {
            pageButtons[i].OnInit(i, this);
        }

        pageButtons[currentIndex].Select();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount >= 2 || !isEnabled) return;

        onBeginDrag?.Invoke(eventData);

        isDragging = true;
        DisableCulling();
        StopSmoothMove();

        previousDragPosition = eventData.position;
        different = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEnabled || !isDragging) return;

        onDrag?.Invoke(eventData);

        if (isCulling)
        {
            DisableCulling();
        }

        Vector2 delta = previousDragPosition - eventData.position;
        previousDragPosition = eventData.position;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            different += delta;
        }

        bool isExceeding = currentIndex == minIndex && delta.x < 0f;
        isExceeding = isExceeding || currentIndex == maxIndex && delta.x > 0f;

        if (!Mathf.Approximately(delta.x, 0f) && (canExceed || !isExceeding))
        {
            root.position = currentRootPosition - new Vector3(different.x / 1.5f, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEnabled || !isDragging) return;

        onEndDrag?.Invoke(eventData);

        float percentage = different.x / Screen.width;

        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            if (percentage > 0 && currentIndex < maxIndex)
            {
                ChangePanel(currentIndex + 1);
            }
            else if (percentage < 0 && currentIndex > minIndex)
            {
                ChangePanel(currentIndex - 1);
            }
            else
            {
                StartSmoothMove(currentRootPosition);
            }
        }
        else
        {
            StartSmoothMove(currentRootPosition);
        }

        isDragging = false;
    }

    private void EnableCulling()
    {
        isCulling = true;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == (currentIndex + 1));
        }
    }

    private void DisableCulling()
    {
        isCulling = false;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(true);
        }
    }

    private void StartSmoothMove(Vector3 endPosition)
    {
        smoothMoveCoroutine = SmoothMoveRoutine(endPosition);
        StartCoroutine(smoothMoveCoroutine);
    }

    private IEnumerator SmoothMoveRoutine(Vector3 endPosition)
    {
        float t = 0f;
        Vector3 startPosition = root.position;

        while (t <= 1.0f)
        {
            t += Time.deltaTime / smoothMoveDuration;
            root.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        EnableCulling();
    }

    private void StopSmoothMove()
    {
        if (smoothMoveCoroutine != null)
        {
            StopCoroutine(smoothMoveCoroutine);
        }
    }

    public void ChangePanel(int nextIndex)
    {
        if (nextIndex == currentIndex || !isEnabled) return;

        StopSmoothMove();
        DisableCulling();
        currentRootPosition += (currentIndex - nextIndex) * Screen.width * Vector3.right;
        StartSmoothMove(currentRootPosition);
        pageButtons[currentIndex].Deselect();
        pageButtons[nextIndex].Select();
        currentIndex = nextIndex;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }
}
