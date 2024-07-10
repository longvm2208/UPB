using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class ScaleTransforms : AnimationBase
{
    [SerializeField] FloatRange range;
    [SerializeField] Transform[] transforms;

    public override void Prepare()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localScale = range.min * Vector3.one;
        }
    }

    public override void Play()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(range.max, duration).SetEase(Ease.OutBack);
        }
    }
}
