using UnityEngine;

public class NormalMilestone : LevelMilestoneBase
{
    [SerializeField] private Sprite previousMilestoneSprite;
    [SerializeField] private Sprite currentMilestoneSprite;
    [SerializeField] private Sprite nextMilestoneSprite;

    public override void Init(int index, int currentIndex, int order, LevelProgress levelProgress)
    {
        base.Init(index, currentIndex, order, levelProgress);

        if (index < currentIndex)
        {
            milestoneImage.sprite = previousMilestoneSprite;
        }
        else if (index == currentIndex)
        {
            milestoneImage.sprite = currentMilestoneSprite;
        }
        else
        {
            milestoneImage.sprite = nextMilestoneSprite;
        }
    }

    public override void ReachingAnimation(float duration)
    {
        base.ReachingAnimation(duration);
        SwitchMilestoneAnimation(duration, nextMilestoneSprite, currentMilestoneSprite);
    }

    public override void PassingAnimation(float duration)
    {
        base.PassingAnimation(duration);
        SwitchMilestoneAnimation(duration, currentMilestoneSprite, previousMilestoneSprite);
    }
}
