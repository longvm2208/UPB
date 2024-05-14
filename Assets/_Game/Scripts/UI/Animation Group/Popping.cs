using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class Popping : AnimationBase
{
    [SerializeField] private FloatRange range;
    [SerializeField] private Transform transform;

    public override void Prepare()
    {
        transform.localScale = range.min * Vector3.one;
    }

    public override void Play()
    {
        transform.DOScale(range.max, duration).SetEase(Ease.OutBack);
    }
}
