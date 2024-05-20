using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int topThreshold = 60;
    [SerializeField, Range(0, 100)]
    private int bottomThreshold = 15;
    [SerializeField, Range(0, 100)]
    private int maxExceeding = 5;
    [SerializeField, Range(0f, 10f)]
    private float animationDuration = 3f;
    [SerializeField] private RectTransform content;
    [SerializeField] private LevelMilestoneBase normalMilestonePrefab;
    [SerializeField] private LevelMilestoneBase hardMilestonePrefab;
    [SerializeField] private AnimationCurve animationCurve;

    private LevelMilestoneBase passingMilestone;
    private LevelMilestoneBase reachingMilestone;

    [Button(ButtonStyle.FoldoutButton)]
    public void Initialize(int currentIndex)
    {
        InitContentHeightAndPosition(currentIndex);

        int start = currentIndex - bottomThreshold - maxExceeding;

        if (start < 0)
        {
            start = 0;
        }

        SpawnMilestones(start, currentIndex);
    }

    private void InitContentHeightAndPosition(int currentIndex)
    {
        bool isExceeding = currentIndex >= bottomThreshold;
        int contentHeight = (isExceeding ? bottomThreshold : currentIndex) + topThreshold;
        int contentPosY = isExceeding ? -bottomThreshold : -currentIndex;
        content.sizeDelta = new Vector2(0f, contentHeight * normalMilestonePrefab.ProgressHeight);
        content.anchoredPosition = new Vector2(0f, contentPosY * normalMilestonePrefab.ProgressHeight);
    }

    private void SpawnMilestones(int start, int currentIndex)
    {
        int order, botExceeding = Mathf.Clamp(currentIndex - bottomThreshold, 0, maxExceeding);
        LevelMilestoneBase milestone;

        for (int i = start; i < currentIndex + topThreshold + maxExceeding; i++)
        {
            //if (gameConfig.IsHardLevel(i))
            //{
            //    milestone = Instantiate(hardMilestonePrefab, content);
            //}
            //else
            //{
            //    milestone = Instantiate(normalMilestonePrefab, content);
            //}

            //order = (i - start) - botExceeding + 1;
            //milestone.Init(i, currentIndex, order, this);

            //if (gameData.isNewLevelAnimation)
            //{
            //    if (i == currentIndex)
            //    {
            //        passingMilestone = milestone;
            //    }
            //    else if (i == currentIndex + 1)
            //    {
            //        reachingMilestone = milestone;
            //    }
            //}
        }
    }

    [Button]
    public void NextLevelAnimation(Action onComplete = null)
    {
        float destinationPosY = content.anchoredPosition.y - normalMilestonePrefab.ProgressHeight;

        content.DOAnchorPosY(destinationPosY, animationDuration).SetEase(animationCurve).OnComplete(() =>
        {
            onComplete?.Invoke();
        });

        passingMilestone.PassingAnimation(animationDuration);
        reachingMilestone.ReachingAnimation(animationDuration);
    }
}
