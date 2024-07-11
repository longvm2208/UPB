using DG.Tweening;
using UnityEngine;

public class BlackPanelBA : BABase
{
    [SerializeField] CanvasGroup canvasGroup;

    public override void Initialize()
    {
        base.Initialize();
        canvasGroup.alpha = 0f;
    }

    public override void FadeIn(float duration)
    {
        canArrangeReward = true;
        canvasGroup.DOFade(1f, duration);
    }

    public override void FadeOut(float duration)
    {
        canvasGroup.DOFade(0f, duration);
    }
}
