using DG.Tweening;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [System.Serializable]
    public class Amount
    {
        [SerializeField, Range(0f, 2f)]
        private float duration = 1f;
        [SerializeField]
        private TMP_Text amountTmp;

        private int previousAmount;

        public void InitAmount(int amount)
        {
            previousAmount = amount;
            amountTmp.text = amount.ToString();
        }

        private void SetAmountTmp(int amount)
        {
            amountTmp.text = amount.ToString();
        }

        public void TweenAmount(int currentAmount)
        {
            DOVirtual.Int(previousAmount, currentAmount, duration, amount =>
            {
                SetAmountTmp(amount);
            });
            previousAmount = currentAmount;
        }
    }

    [System.Serializable]
    public class Icon
    {
        [SerializeField, Range(0f, 1f)]
        private float duration = 1f;
        [SerializeField, Range(1f, 2f)]
        private float scale = 1.2f;
        [SerializeField]
        private Transform transform;
        [SerializeField]
        private AnimationCurve curve;

        public void TweenIcon()
        {
            DOTween.Complete(transform);
            transform.DOScale(scale, duration).SetEase(curve);
        }
    }

    [SerializeField]
    private Amount amount;
    [SerializeField]
    private Icon icon;
}
