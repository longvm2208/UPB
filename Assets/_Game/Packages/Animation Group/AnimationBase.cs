using System;
using UnityEngine;

[Serializable]
public abstract class AnimationBase : IElement
{
    [SerializeField] protected float duration = 0.5f;

    public float Duration => duration;

    public abstract void Prepare();
    public abstract void Play();
}
