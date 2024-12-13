using UnityEngine;

public abstract class PopupWithCloseAnimation : PopupBase
{
    [SerializeField] protected AnimationGroup animationGroup;

    public override void Close()
    {
        animationGroup.Play(() =>
        {
            UIManager.Ins.OnPopupClose();
            gameObject.SetActive(false);
        });
    }
}
