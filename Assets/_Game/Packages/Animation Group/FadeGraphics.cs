using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeGraphics : AnimationBase
{
    [SerializeField] FloatRange range;
    [SerializeField] Graphic[] graphics;

    public override void Prepare()
    {
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].ChangeAlpha(range.min);
        }
    }

    public override void Play()
    {
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].DOFade(range.max, duration);
        }
    }
}
