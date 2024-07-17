using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

public class IAPButtonPurchase : MonoBehaviour
{
    [SerializeField, ValueDropdown("ids")] string id;
    [SerializeField] IAPLocation location;
    [SerializeField] TMP_Text priceTmp;

    static string[] ids = IAPProductId.Ids;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => IAPManager.Instance.IsInitialized);

        priceTmp.text = IAPManager.Instance.GetLocalizedPriceString(id);
    }

    #region UI Events
    public void OnClick()
    {
        IAPManager.Instance.OnPurchaseClicked(id, location);
    }
    #endregion
}
