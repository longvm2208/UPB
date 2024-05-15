using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphic : AnimationBase
{
    [SerializeField] private FloatRange range;
    [SerializeField] private Graphic graphic;

    public override void Prepare()
    {
        graphic.ChangeAlpha(range.min);
    }

    public override void Play()
    {
        graphic.DOFade(range.max, duration);
    }
}
