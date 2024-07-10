using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class MoveRectTransform : AnimationBase
{
    [SerializeField] Vector3 start;
    [SerializeField] Vector3 end;
    [SerializeField] RectTransform rectTransform;

    public override void Prepare()
    {
        rectTransform.anchoredPosition = start;
    }

    public override void Play()
    {
        rectTransform.DOAnchorPos(end, duration);
    }
}
