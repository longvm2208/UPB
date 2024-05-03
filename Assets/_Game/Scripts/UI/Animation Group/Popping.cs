using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class Popping : AnimationBase
{
    [SerializeField] private Transform transform;

    public override void Init(int state)
    {
        transform.localScale = state * Vector3.one;
    }

    public override void Forward()
    {
        transform.DOScale(1f, duration).SetEase(Ease.OutBack);
    }

    public override void Backward()
    {
        transform.DOScale(0f, duration);
    }
}
