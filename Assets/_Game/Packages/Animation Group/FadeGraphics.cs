using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeGraphics : AnimationBase
{
    [SerializeField] Vector2 range;
    [SerializeField] Graphic[] graphics;

    public override void Prepare()
    {
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].ChangeAlpha(range.x);
        }
    }

    public override void Play()
    {
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].DOFade(range.y, duration);
        }
    }
}
