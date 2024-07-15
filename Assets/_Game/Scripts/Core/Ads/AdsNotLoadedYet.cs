using DG.Tweening;
using UnityEngine;

public class AdsNotLoadedYet : MonoBehaviour
{
    [SerializeField] float startPosY = -200f;
    [SerializeField] float endPosY = 200f;
    [SerializeField] float duration = 1f;
    [SerializeField] RectTransform myTransform;

    private void OnValidate()
    {
        if (myTransform == null)
        {
            myTransform = transform as RectTransform;
        }
    }

    private void OnEnable()
    {
        myTransform.ChangeAnchorPosY(startPosY);
        myTransform.DOAnchorPosY(endPosY, duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
