using System;

[Serializable]
public class CurrencyModifier : ModifierBase
{
    public override string Modify(int amount)
    {
        return string.Format(format, amount);
    }
}
