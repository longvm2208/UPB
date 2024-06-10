using UnityEngine;

public class NormalPoint : PointBase
{
    [SerializeField] private Sprite previousSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Sprite nextSprite;

    public override void Init(int index, int currentIndex, int order, float progressHeight, MainProgress mainProgress)
    {
        base.Init(index, currentIndex, order, progressHeight, mainProgress);

        if (index < currentIndex)
        {
            indexHolderImage.sprite = previousSprite;
        }
        else if (index == currentIndex)
        {
            indexHolderImage.sprite = currentSprite;
        }
        else
        {
            indexHolderImage.sprite = nextSprite;
        }
    }

    public override void ReachingAnimation(float duration)
    {
        base.ReachingAnimation(duration);
        SwitchIndexHolderSpriteAnimation(duration, nextSprite, currentSprite);
    }

    public override void PassingAnimation(float duration)
    {
        base.PassingAnimation(duration);
        SwitchIndexHolderSpriteAnimation(duration, currentSprite, previousSprite);
    }
}
