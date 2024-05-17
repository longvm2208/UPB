using System;
using UnityEngine;

public abstract class RewardBase : MonoBehaviour
{
    protected Action onReceiveReward;
    protected Action onComplete;

    public abstract RewardBase OnInit(int amount);
    public abstract RewardBase Play(float duration);

    public virtual RewardBase OnReceiveReward(Action onReceiveReward)
    {
        this.onReceiveReward = onReceiveReward;
        return this;
    }

    public virtual RewardBase OnComplete(Action onComplete)
    {
        this.onComplete = onComplete;
        return this;
    }
}
