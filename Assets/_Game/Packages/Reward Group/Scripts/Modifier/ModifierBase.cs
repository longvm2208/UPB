using System;
using UnityEngine;

[Serializable]
public abstract class ModifierBase
{
    [SerializeField] protected string format = "{0}";

    public abstract string Modify(int amount);
}
