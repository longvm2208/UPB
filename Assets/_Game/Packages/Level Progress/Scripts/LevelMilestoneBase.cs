using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelMilestoneBase : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    protected float normalScale = 1f;
    [SerializeField, Range(0f, 10f)]
    protected float highlightScale = 1.3f;
    [SerializeField] protected RectTransform myTransform;
    [SerializeField] protected RectTransform progressTransform;
    [SerializeField] protected RectTransform milestoneTransform;
    [SerializeField] protected TMP_Text levelTmp;
    [SerializeField] protected Image fillImage, milestoneImage;
    [SerializeField] protected Image milestonePrefab;
    [SerializeField] protected TMP_Text levelTmpPrefab;

    protected LevelProgress levelProgress;

    public float ProgressHeight => progressTransform.sizeDelta.y;

    public virtual void Init(int index, int currentIndex, int order, LevelProgress levelProgress)
    {
        this.levelProgress = levelProgress;
        levelTmp.text = (index + 1).ToString();
        myTransform.anchoredPosition = order * ProgressHeight * Vector2.up;
        fillImage.fillAmount = index < currentIndex ? 1f : 0f;

        if (index == currentIndex)
        {
            milestoneTransform.localScale = highlightScale * Vector3.one;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public virtual void PassingAnimation(float duration)
    {
        milestoneTransform.DOScale(1f, duration);
        ProgressPassingAnimation(duration);
    }

    [Button(ButtonStyle.FoldoutButton)]
    public virtual void ReachingAnimation(float duration)
    {
        milestoneTransform.DOScale(highlightScale, duration);
    }

    protected virtual void ProgressPassingAnimation(float duration)
    {
        DOVirtual.Float(0f, 1f, duration, fillValue =>
        {
            fillImage.fillAmount = fillValue;
        });
    }

    [Button(ButtonStyle.FoldoutButton)]
    protected void SwitchMilestoneAnimation(float duration, Sprite from, Sprite to)
    {
        milestoneImage.sprite = to;
        Image temp = Instantiate(milestonePrefab, milestoneTransform);
        temp.transform.SetSiblingIndex(0);
        temp.sprite = from;
        temp.DOFade(0f, duration).OnComplete(() =>
        {
            Destroy(temp.gameObject);
        });
    }

    protected void SwitchLevelTmpAnimation(float duration, TMP_FontAsset from, TMP_FontAsset to)
    {
        levelTmp.font = to;
        TMP_Text temp = Instantiate(levelTmpPrefab, milestoneTransform);
        temp.transform.SetSiblingIndex(2);
        temp.font = from;
        temp.DOFade(0f, duration).OnComplete(() =>
        {
            Destroy(temp.gameObject);
        });
    }
}
