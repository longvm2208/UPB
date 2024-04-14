using UnityEngine;
using TMPro;

namespace Cheat
{
    public class CheatPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField levelInputField;
        [SerializeField]
        private TMP_InputField coinInputField;

        public void OnClickLoadLevel()
        {
            if (int.TryParse(levelInputField.text, out int level))
            {
                Debug.Log(level);
            }
        }

        public void OnClickShowAndHideUI()
        {
            Debug.Log("Show and hide UI");
        }

        public void OnClickAddCoin()
        {
            if (int.TryParse(coinInputField.text, out int amount))
            {
                Debug.Log($"Add {amount} coin");
            }
        }
    }
}
