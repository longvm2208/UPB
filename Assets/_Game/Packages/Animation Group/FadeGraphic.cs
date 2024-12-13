using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeGraphic : AnimationBase
{
    [SerializeField] Vector2 range;
    [SerializeField] Graphic graphic;

    public override void Prepare()
    {
        graphic.ChangeAlpha(range.x);
    }

    public override void Play()
    {
        graphic.DOFade(range.y, duration);
    }
}
