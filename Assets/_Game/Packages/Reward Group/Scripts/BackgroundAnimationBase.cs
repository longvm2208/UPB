using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //[SerializeField] private float targetBackgroundAl
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text tapToClaimText;

    public override BackgroundAnimationBase Initialize()
    {
        backgroundImage.gameObject.SetActive(true);
        backgroundImage.ChangeAlpha(0f);
        tapToClaimText.ChangeAlpha(0f);

        return this;
    }

    public override BackgroundAnimationBase PlayFadeInAnimation(float duration)
    {
        //backgroundImage.

        return this;
    }

    public override BackgroundAnimationBase PlayFadeOutAnimation(float duration)
    {
        return this;
    }
}
