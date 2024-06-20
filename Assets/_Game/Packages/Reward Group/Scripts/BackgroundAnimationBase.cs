using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class BackgroundAnimationBase : MonoBehaviour
{
    protected Action onArrangeReward;

    [Button(ButtonStyle.FoldoutButton)]
    public abstract BackgroundAnimationBase Initialize();

    [Button(ButtonStyle.FoldoutButton)]
    public abstract BackgroundAnimationBase PlayFadeInAnimation(float duration);

    [Button(ButtonStyle.FoldoutButton)]
    public abstract BackgroundAnimationBase PlayFadeOutAnimation(float duration);

    public BackgroundAnimationBase OnArrangeReward(Action onArrangeReward)
    {
        this.onArrangeReward = onArrangeReward;

        return this;
    }
}

public class BackgroundAnimation1 : BackgroundAnimationBase
{
    public override BackgroundAnimationBase Initialize()
    {
        throw new NotImplementedException();
    }

    public override BackgroundAnimationBase PlayFadeInAnimation(float duration)
    {
        throw new NotImplementedException();
    }

    public override BackgroundAnimationBase PlayFadeOutAnimation(float duration)
    {
        throw new NotImplementedException();
    }
}
