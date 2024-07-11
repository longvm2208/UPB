using DG.Tweening;
using TMPro;
using UnityEngine;

public class SingleIconRA : RABase
{
    [SerializeField] Transform destination;
    [SerializeField] float targetScale = 0.5f;
    [SerializeField] TMP_Text amountTmp;
    [SerializeField] RectTransform iconTransform;
    [SerializeReference] ModifierBase modifier = new AmountModifier();

    public override void SetDestination(Transform destination)
    {
        this.destination = destination;
    }

    public override RABase Initialize(int amount)
    {
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = modifier.Modify(amount);
        iconTransform.localPosition = Vector3.zero;
        iconTransform.localScale = Vector3.one;

        return this;
    }

    public override void Play(float duration)
    {
        amountTmp.gameObject.SetActive(false);
        iconTransform.DOScale(targetScale, duration);

        iconTransform.DOMove(destination.position, duration).SetEase(Ease.InCubic).OnComplete(() =>
        {
            onReceiveReward?.Invoke();
            onComplete?.Invoke();
        });
    }
}
