using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class Moving : AnimationBase
{
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private RectTransform rectTransform;

    public override void Prepare()
    {
        rectTransform.anchoredPosition = start;
    }

    public override void Play()
    {
        rectTransform.DOAnchorPos(end, duration);
    }
}
