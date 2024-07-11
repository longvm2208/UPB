using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircleAA : AABase
{
    [SerializeField] float[] radiuses;
    [SerializeField] Vector2 center;

    public override void Initialize(List<RectTransform> actives)
    {
        base.Initialize(actives);

        if (actives.IsNullOrEmpty()) return;

        for (int i = 0; i < actives.Count; i++)
        {
            actives[i].localScale = Vector3.zero;
        }

        if (actives.Count == 1)
        {
            actives[0].localPosition = center;
            return;
        }

        float angleModifier = 90f;

        if (actives.Count == 2)
        {
            angleModifier = 0f;
        }

        float radius = radiuses[actives.Count - 1];
        float angleStep = 360f / actives.Count;

        for (int i = 0; i < actives.Count; i++)
        {
            float angle = i * angleStep + angleModifier;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            actives[i].localPosition = center + new Vector2(x, y);
        }
    }

    public override void Play(float duration)
    {
        for (int i = 0; i < actives.Count; i++)
        {
            actives[i].DOScale(1f, duration).SetEase(Ease.OutBack);
        }

        ScheduleUtils.DelayTask(duration, () =>
        {
            isComplete = true;
        });
    }
}
