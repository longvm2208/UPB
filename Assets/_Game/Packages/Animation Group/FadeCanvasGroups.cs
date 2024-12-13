using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class FadeCanvasGroups : AnimationBase
{
    [SerializeField] Vector2 range;
    [SerializeField] CanvasGroup[] canvasGroups;

    public override void Prepare()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = range.x;
        }
    }

    public override void Play()
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].DOFade(range.y, duration);
        }
    }
}
