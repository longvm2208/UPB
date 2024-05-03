using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class Fade : AnimationBase
{
    [SerializeField] private CanvasGroup canvasGroup;

    public override void Init(int state)
    {
        canvasGroup.alpha = state;
    }

    public override void Forward()
    {
        canvasGroup.DOFade(1f, duration);
    }

    public override void Backward()
    {
        canvasGroup.DOFade(0f, duration);
    }
}
