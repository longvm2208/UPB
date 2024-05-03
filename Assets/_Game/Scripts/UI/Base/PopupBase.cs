using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField] protected RectTransform myTransform;

    public virtual void Open(object args = null)
    {
        UIManager.Instance.OnPopupOpen();
        gameObject.SetActive(true);
        myTransform.SetAsLastSibling();
    }

    public virtual void Close()
    {
        UIManager.Instance.OnPopupClose();
        gameObject.SetActive(false);
    }
}
