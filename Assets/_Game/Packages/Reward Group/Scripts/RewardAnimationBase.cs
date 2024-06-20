using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class RewardAnimationBase : MonoBehaviour
{
    protected Transform destination;
    protected Action onReceiveReward;
    protected Action onComplete;

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RewardAnimationBase Initialize(int amount);

    [Button(ButtonStyle.FoldoutButton)]
    public virtual RewardAnimationBase SetDestination(Transform destination)
    {
        this.destination = destination;

        return this;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RewardAnimationBase PlayAppearAnimation(float duration);

    [Button(ButtonStyle.FoldoutButton)]
    public abstract RewardAnimationBase PlayClaimAnimation(float duration);

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
