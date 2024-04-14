using UnityEngine;

[CreateAssetMenu(fileName = "IAP Product Container", menuName = "Scriptable Objects/IAP Product Containter")]
public class IAPProductContainer : ScriptableObject
{
    public IAPProduct[] products;

    public int Count => products.Length;

    public IAPProduct this[int i] => products[i];
}
