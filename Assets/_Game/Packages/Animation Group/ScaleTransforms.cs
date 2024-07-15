using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class ScaleTransforms : AnimationBase
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    [SerializeField] Transform[] transforms;
    [SerializeField] Ease ease;

    public override void Prepare()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localScale = start;
        }
    }

    public override void Play()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].DOScale(end, duration).SetEase(ease);
        }
    }
}
