using UnityEngine;

public class HardMilestone : LevelMilestoneBase
{
    [SerializeField] private Sprite previousMilestoneSprite;
    [SerializeField] private Sprite currentMilestoneSprite;

    public override void Init(int index, int currentIndex, int order, LevelProgress levelProgress)
    {
        base.Init(index, currentIndex, order, levelProgress);

        milestoneImage.sprite = index < currentIndex ?
            previousMilestoneSprite : currentMilestoneSprite;
    }

    public override void PassingAnimation(float duration)
    {
        base.PassingAnimation(duration);
        SwitchMilestoneAnimation(duration, currentMilestoneSprite, previousMilestoneSprite);
    }
}
