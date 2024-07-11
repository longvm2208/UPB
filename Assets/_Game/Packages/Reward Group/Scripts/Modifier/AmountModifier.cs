using System;

[Serializable]
public class AmountModifier : ModifierBase
{
    public override string Modify(int amount)
    {
        return string.Format(format, amount);
    }
}
