using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeGraphic : AnimationBase
{
    [SerializeField] FloatRange range;
    [SerializeField] Graphic graphic;

    public override void Prepare()
    {
        graphic.ChangeAlpha(range.min);
    }

    public override void Play()
    {
        graphic.DOFade(range.max, duration);
    }
}
