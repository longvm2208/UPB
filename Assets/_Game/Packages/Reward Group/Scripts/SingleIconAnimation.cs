using DG.Tweening;
using TMPro;
using UnityEngine;

public class SingleIconAnimation : RewardAnimationBase
{
    [SerializeReference] private ModifierBase modifier = new CurrencyModifier();
    [SerializeField] private float targetScale = 0.5f;
    [SerializeField] private TMP_Text amountTmp;
    [SerializeField] private RectTransform iconTransform;

    public override RewardAnimationBase Initialize(int amount)
    {
        gameObject.SetActive(true);
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = modifier.Modify(amount);
        iconTransform.localPosition = Vector3.zero;
        iconTransform.localScale = Vector3.zero;

        return this;
    }

    public override RewardAnimationBase PlayAppearAnimation(float duration)
    {
        iconTransform.DOScale(1f, duration);

        return this;
    }

    public override RewardAnimationBase PlayClaimAnimation(float duration)
    {
        amountTmp.gameObject.SetActive(false);
        iconTransform.DOScale(targetScale, duration);

        iconTransform.DOMove(destination.position, duration).OnComplete(() =>
        {
            onReceiveReward?.Invoke();
            onComplete?.Invoke();
        });

        return this;
    }
}
