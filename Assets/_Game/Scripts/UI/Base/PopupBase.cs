using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField] protected RectTransform myTransform;

    protected virtual void OnValidate()
    {
        if (myTransform == null)
        {
            myTransform = GetComponent<RectTransform>();
        }
    }

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
