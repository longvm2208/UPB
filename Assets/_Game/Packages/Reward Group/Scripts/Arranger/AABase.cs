using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AABase
{
    protected bool isComplete;
    protected List<RectTransform> actives;

    public bool IsComplete => isComplete;

    public virtual void Initialize(List<RectTransform> actives)
    {
        this.actives = actives;
        isComplete = false;
    }

    public abstract void Play(float duration);
}
