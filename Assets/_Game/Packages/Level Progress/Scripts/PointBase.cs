using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PointBase : MonoBehaviour
{
    [SerializeField] protected Vector2 indexHolderScaleRange = new Vector2(1f, 1.3f);
    [SerializeField] protected RectTransform myTransform;
    [SerializeField] protected RectTransform progressTransform;
    [SerializeField] protected RectTransform indexHolderTransform;
    [SerializeField] protected TMP_Text indexTmp;
    [SerializeField] protected Image progressFillImage;
    [SerializeField] protected Image indexHolderImage;
    [SerializeField] protected Image imagePrefab;

    protected MainProgress mainProgress;

    private void OnValidate()
    {
        if (indexHolderImage == null && indexHolderTransform != null)
        {
            indexHolderImage = indexHolderTransform.GetComponent<Image>();
        }
    }

    public virtual void Init(int index, int currentIndex, int order, float progressHeight, MainProgress mainProgress)
    {
        this.mainProgress = mainProgress;
        indexTmp.text = (index + 1).ToString();
        progressTransform.ChangeSizeDeltaY(progressHeight);
        myTransform.anchoredPosition = order * progressHeight * Vector2.up;
        progressFillImage.fillAmount = index < currentIndex ? 1f : 0f;
        float scale = index == currentIndex ? indexHolderScaleRange.y : indexHolderScaleRange.x;
        indexHolderTransform.localScale = scale * Vector3.one;
    }

    [Button(ButtonStyle.FoldoutButton)]
    public virtual void PassingAnimation(float duration)
    {
        indexHolderTransform.DOScale(indexHolderScaleRange.x, duration);
        ProgressPassingAnimation(duration);
    }

    [Button(ButtonStyle.FoldoutButton)]
    public virtual void ReachingAnimation(float duration)
    {
        indexHolderTransform.DOScale(indexHolderScaleRange.y, duration);
    }

    protected virtual void ProgressPassingAnimation(float duration)
    {
        DOVirtual.Float(0f, 1f, duration, fillValue =>
        {
            progressFillImage.fillAmount = fillValue;
        });
    }

    [Button(ButtonStyle.FoldoutButton)]
    protected void SwitchIndexHolderSpriteAnimation(float duration, Sprite from, Sprite to)
    {
        indexHolderImage.sprite = to;
        Image temp = Instantiate(imagePrefab, indexHolderTransform);
        temp.transform.SetSiblingIndex(0);
        temp.sprite = from;
        temp.DOFade(0f, duration).OnComplete(() =>
        {
            Destroy(temp.gameObject);
        });
    }
}
