using UnityEngine;

namespace CustomTabSystem
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        private TabButton[] tabButtons;
        [SerializeField]
        private GameObject[] contents;

        private int selectedIndex = -1;

        public void SelectTab(int index)
        {
            if (selectedIndex != -1)
            {
                tabButtons[selectedIndex].Deselect();
                contents[selectedIndex].SetActive(false);
            }

            selectedIndex = index;
            tabButtons[selectedIndex].Select();
            contents[selectedIndex].SetActive(true);
        }
    }
}
