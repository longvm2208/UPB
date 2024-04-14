using UnityEngine;

namespace Cheat
{
    public class CheatConsole : MonoBehaviour
    {
        [SerializeField]
        private bool isHideButton;
        [SerializeField]
        private CheatButton cheatButton;
        [SerializeField]
        private GameObject cheatPanel;

        public bool IsHideButton => isHideButton;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void OpenCheatPanel()
        {
            cheatPanel.SetActive(true);
        }

        public void CloseCheatPanel()
        {
            cheatPanel.SetActive(false);

            if (isHideButton)
            {
                cheatButton.ShowButtonTemporarily();
            }
        }
    }
}
