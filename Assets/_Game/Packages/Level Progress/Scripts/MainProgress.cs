using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class MainProgress : MonoBehaviour
{
    [SerializeField, Range(0, 40)]
    private int topThreshold = 10;
    [SerializeField, Range(0, 20)]
    private int bottomThreshold = 5;
    [SerializeField, Range(0, 20)]
    private int maxExceeding = 5;
    [SerializeField, Range(0f, 1600f)]
    private float pointDistance = 400f;
    [SerializeField, Range(0f, 4f)]
    private float newLevelAnimationDuration = 1f;
    [SerializeField] private RectTransform content;
    [SerializeField] private PointBase normalPointPrefab;
    [SerializeField] private PointBase hardPointPrefab;

    private PointBase passingPoint;
    private PointBase reachingPoint;

    public bool IsNewLevelAnimationComplete { get; private set; }

    [Button(ButtonStyle.FoldoutButton)]
    public void Initialize(int currentIndex)
    {
        SetupContentHeightAndPosition(currentIndex);

        int start = currentIndex - bottomThreshold - maxExceeding;

        if (start < 0)
        {
            start = 0;
        }

        SpawnPoints(start, currentIndex);
    }

    private void SetupContentHeightAndPosition(int currentIndex)
    {
        int belowPointAmount = currentIndex >= bottomThreshold ? bottomThreshold : currentIndex;
        int pointAmount = belowPointAmount + topThreshold;
        content.sizeDelta = new Vector2(0f, pointAmount * pointDistance);
        content.anchoredPosition = new Vector2(0f, -belowPointAmount * pointDistance);
    }

    private void SpawnPoints(int start, int currentIndex)
    {
        int bottomExceeding = Mathf.Clamp(currentIndex - bottomThreshold, 0, maxExceeding);

        for (int i = start; i < currentIndex + topThreshold + maxExceeding; i++)
        {
            PointBase point = Instantiate(GetPointPrefabByIndex(i), content);
            int order = (i - start) - bottomExceeding + 1;
            point.Init(i, currentIndex, order, pointDistance, this);

            if (i == currentIndex)
            {
                passingPoint = point;
            }
            else if (i == currentIndex + 1)
            {
                reachingPoint = point;
            }
        }
    }

    private PointBase GetPointPrefabByIndex(int i)
    {
        //if (gameConfig.IsHardLevel(i))
        //{
        //    return hardPointPrefab;
        //}
        //else
        //{
        //    return normalPointPrefab;
        //}

        return normalPointPrefab;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void PlayNextLevelAnimation(Action onComplete = null)
    {
        float destinationPosY = content.anchoredPosition.y - pointDistance;

        content.DOAnchorPosY(destinationPosY, newLevelAnimationDuration).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            IsNewLevelAnimationComplete = true;
            onComplete?.Invoke();
        });

        passingPoint.PassingAnimation(newLevelAnimationDuration);
        reachingPoint.ReachingAnimation(newLevelAnimationDuration);
    }

    [Button]
    public void Clear()
    {
        content.DestroyChildren();
    }
}
