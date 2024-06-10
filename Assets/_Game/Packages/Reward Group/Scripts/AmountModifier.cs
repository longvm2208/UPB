using System;
using UnityEngine;

[Serializable]
public class AmountModifier
{
    [SerializeField] protected string format = "x{0}";

    public virtual string Modify(int amount)
    {
        return string.Format(format, amount);
    }
}
