using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class RewardAnimationBase : MonoBehaviour
{
    protected Transform destination;
    protected Action onReceiveReward;
    protected Action onComplete;

    [Button(ButtonStyle.FoldoutButton)]
    public void SetDestination(Transform destination)
    {
        this.destination = destination;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RewardAnimationBase Init(int amount);

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RewardAnimationBase Play(float duration);

    protected virtual string ModifyAmount(int amount)
    {
        return amount.ToString();
    }

    public virtual RewardAnimationBase OnReceiveReward(Action onReceiveReward)
    {
        this.onReceiveReward = onReceiveReward;
        return this;
    }

    public virtual RewardAnimationBase OnComplete(Action onComplete)
    {
        this.onComplete = onComplete;
        return this;
    }
}
