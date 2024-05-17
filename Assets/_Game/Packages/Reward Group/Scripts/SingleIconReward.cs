using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class SingleIconReward : RewardBase
{
    [SerializeField] private string amountFormat = "x{0}";
    [SerializeField] private TMP_Text amountTmp;
    [SerializeField] private RectTransform iconTransform;
    [SerializeField] private RectTransform destination;

    public override RewardBase OnInit(int amount)
    {
        gameObject.SetActive(true);
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = string.Format(amountFormat, amount);
        iconTransform.localPosition = Vector3.zero;
        iconTransform.localScale = Vector3.one;
        iconTransform.DOScale(1f, 0.5f);
        return this;
    }

    public override RewardBase Play(float duration)
    {
        amountTmp.gameObject.SetActive(false);
        iconTransform.DOMove(destination.position, duration);
        return this;
    }
}
