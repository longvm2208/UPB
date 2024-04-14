using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class IAPButtonPurchase : MonoBehaviour
{
    [SerializeField, ValueDropdown("ids")]
    private string id;
    [SerializeField]
    private TMP_Text priceTmp;

    private static string[] ids = IAPProductId.Ids;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => IAPManager.Instance.IsInitialized);

        priceTmp.text = IAPManager.Instance.GetLocalizedPriceString(id);
    }

    #region UI Events
    public void OnClick()
    {
        IAPManager.Instance.OnPurchaseClicked(id);
    }
    #endregion
}
