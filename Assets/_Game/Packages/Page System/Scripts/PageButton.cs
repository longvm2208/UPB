using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PageButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;

    private int index;
    private PageSwipe pageSwiper;

    public void OnInit(int index, PageSwipe pageSwiper)
    {
        this.index = index;
        this.pageSwiper = pageSwiper;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        pageSwiper.ChangePanel(index);
    }

    public void Select()
    {
        onSelected?.Invoke();
    }

    public void Deselect()
    {
        onDeselected?.Invoke();
    }
}
