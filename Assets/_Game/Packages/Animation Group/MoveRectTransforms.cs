using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class MoveRectTransforms : AnimationBase
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    [SerializeField] RectTransform[] rectTransforms;

    public override void Prepare()
    {
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i].anchoredPosition = start;
        }
    }

    public override void Play()
    {
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i].DOAnchorPos(end, duration);
        }
    }
}
