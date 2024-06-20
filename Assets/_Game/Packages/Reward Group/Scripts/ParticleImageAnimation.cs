using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ParticleImageAnimation : RewardAnimationBase
{
    [SerializeReference] private ModifierBase modifier = new CurrencyModifier();
    [SerializeField] private TMP_Text amountTmp;
    [SerializeField] private RectTransform iconTransform;
    [SerializeField] private ParticleImage particleImage;

    public override RewardAnimationBase Initialize(int amount)
    {
        gameObject.SetActive(true);
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = modifier.Modify(amount);
        iconTransform.localPosition = Vector3.zero;
        iconTransform.localScale = Vector3.zero;

        return this;
    }

    public override RewardAnimationBase SetDestination(Transform destination)
    {
        particleImage.attractorTarget = destination;

        return base.SetDestination(destination);
    }

    public override RewardAnimationBase PlayAppearAnimation(float duration)
    {
        iconTransform.DOScale(1f, duration);

        return this;
    }

    public override RewardAnimationBase PlayClaimAnimation(float duration)
    {
        iconTransform.DOScale(0f, 0.5f);
        particleImage.Play();

        return this;
    }

    public void OnFirstParticleFinish() => onReceiveReward?.Invoke();
    public void OnLastParticleFinish() => onComplete?.Invoke();
}
