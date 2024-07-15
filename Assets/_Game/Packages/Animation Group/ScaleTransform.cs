using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class ScaleTransform : AnimationBase
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    [SerializeField] Transform transform;
    [SerializeField] Ease ease;

    public override void Prepare()
    {
        transform.localScale = start;
    }

    public override void Play()
    {
        transform.DOScale(end, duration).SetEase(ease);
    }
}
