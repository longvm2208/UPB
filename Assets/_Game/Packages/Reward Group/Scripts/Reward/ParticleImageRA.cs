using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ParticleImageRA : RABase
{
    [SerializeField] TMP_Text amountTmp;
    [SerializeField] RectTransform iconTransform;
    [SerializeField] ParticleImage particleImage;
    [SerializeReference] ModifierBase modifier = new AmountModifier();

    public override void SetDestination(Transform destination)
    {
        particleImage.attractorTarget = destination;
    }

    public override RABase Initialize(int amount)
    {
        amountTmp.gameObject.SetActive(true);
        amountTmp.text = modifier.Modify(amount);
        iconTransform.localScale = Vector3.one;

        return this;
    }

    public override void Play(float duration)
    {
        iconTransform.DOScale(0f, 0.5f);
        particleImage.Play();
    }

    public void OnFirstParticleFinish() => onReceiveReward?.Invoke();
    public void OnLastParticleFinish() => onComplete?.Invoke();
}
