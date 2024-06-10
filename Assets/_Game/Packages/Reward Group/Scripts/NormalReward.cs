using DG.Tweening;
using TMPro;
using UnityEngine;

public class NormalReward : RewardAnimationBase
{
    [SerializeReference] private AmountModifier amountModifier = new AmountModifier();
    [SerializeField] private float targetScale = 0.5f;
    [SerializeField] private TMP_Text amountTmp;
    [SerializeField] private RectTransform iconTransform;

    public override RewardAnimationBase Init(int amount)
    {
        gameObject.SetActive(true);
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = amountModifier.Modify(amount);
        iconTransform.localPosition = Vector3.zero;
        iconTransform.localScale = Vector3.zero;
        iconTransform.DOScale(1f, 0.5f);
        return this;
    }

    public override RewardAnimationBase Play(float duration)
    {
        amountTmp.gameObject.SetActive(false);
        iconTransform.DOMove(destination.position, duration);
        iconTransform.DOScale(targetScale, duration);
        return this;
    }
}
