using UnityEngine;

public class HardPoint : PointBase
{
    [SerializeField] private Sprite previousSprite;
    [SerializeField] private Sprite currentSprite;

    public override void Init(int index, int currentIndex, int order, float progressHeight, MainProgress mainProgress)
    {
        base.Init(index, currentIndex, order, progressHeight, mainProgress);

        indexHolderImage.sprite = index < currentIndex ? previousSprite : currentSprite;
    }

    public override void PassingAnimation(float duration)
    {
        base.PassingAnimation(duration);
        SwitchIndexHolderSpriteAnimation(duration, currentSprite, previousSprite);
    }
}
