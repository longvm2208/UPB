using Sirenix.OdinInspector;
using UnityEngine.Purchasing;

[System.Serializable]
public struct IAPProduct
{
    [ValueDropdown("ids"), HorizontalGroup("row"), HideLabel]
    public string id;
    [HorizontalGroup("row"), HideLabel]
    public ProductType type;

    private static string[] ids = IAPProductId.Ids;
}
