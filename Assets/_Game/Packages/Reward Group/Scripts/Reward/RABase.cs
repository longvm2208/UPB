using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class RABase : MonoBehaviour
{
    [SerializeField] RectTransform myTransform;

    protected Action onReceiveReward;
    protected Action onComplete;

    public RectTransform MyTransform => myTransform;

    private void OnValidate()
    {
        if (myTransform == null)
        {
            myTransform = transform as RectTransform;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public abstract void SetDestination(Transform destination);

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RABase Initialize(int amount);

    public virtual RABase OnReceiveReward(Action onReceiveReward)
    {
        this.onReceiveReward = onReceiveReward;
        return this;
    }

    public virtual RABase OnComplete(Action onComplete)
    {
        this.onComplete = onComplete;
        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public abstract void Play(float duration);
}
