using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomTabSystem
{
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private int index;
        [SerializeField]
        private TabGroup tabGroup;

        private bool isSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSelected)
            {
                return;
            }

            tabGroup.SelectTab(index);
        }

        public void Select()
        {
            isSelected = true;
        }

        public void Deselect()
        {
            isSelected = false;
        }
    }
}

