using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class FadeCanvasGroup : AnimationBase
{
    [SerializeField] FloatRange range;
    [SerializeField] CanvasGroup canvasGroup;

    public override void Prepare()
    {
        canvasGroup.alpha = range.min;
    }

    public override void Play()
    {
        canvasGroup.DOFade(range.max, duration);
    }
}
