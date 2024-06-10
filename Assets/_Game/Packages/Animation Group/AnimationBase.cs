using System;
using UnityEngine;

[Serializable]
public abstract class AnimationBase
{
    [SerializeField] protected float interval = 0.25f;
    [SerializeField] protected float duration = 0.5f;

    public float Interval => interval;
    public float Duration => duration;

    public abstract void Prepare();
    public abstract void Play();
}
