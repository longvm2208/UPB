using UnityEngine;

[RequireComponent(typeof(AnimationGroup))]
public abstract class PopupWithCloseAnimation : PopupBase
{
    [SerializeField] protected AnimationGroup animationGroup;

    protected override void OnValidate()
    {
        animationGroup = GetComponent<AnimationGroup>();
    }

    public override void Close()
    {
        animationGroup.Play(() =>
        {
            UIManager.Instance.OnPopupClose();
            gameObject.SetActive(false);
        });
    }
}
