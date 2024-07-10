using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class FadeCanvasGroups : AnimationBase
{
    [SerializeField] FloatRange range;
    [SerializeField] CanvasGroup[] canvasGroups;

    public override void Prepare()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = range.min;
        }
    }

    public override void Play()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].DOFade(range.max, duration);
        }
    }
}
