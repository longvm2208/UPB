using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class Fade : AnimationBase
{
    [SerializeField] private FloatRange range;
    [SerializeField] private CanvasGroup canvasGroup;

    public override void Prepare()
    {
        canvasGroup.alpha = range.min;
    }

    public override void Play()
    {
        canvasGroup.DOFade(range.max, duration);
    }
}
