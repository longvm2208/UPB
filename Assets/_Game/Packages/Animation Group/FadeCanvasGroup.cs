using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class FadeCanvasGroup : AnimationBase
{
    [SerializeField] Vector2 range;
    [SerializeField] CanvasGroup canvasGroup;

    public override void Prepare()
    {
        canvasGroup.alpha = range.x;
    }

    public override void Play()
    {
        canvasGroup.DOFade(range.y, duration);
    }
}
