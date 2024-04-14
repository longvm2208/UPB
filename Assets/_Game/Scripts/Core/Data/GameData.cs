using System;

[Serializable]
public partial class GameData
{
    public AudioData Audio;
    public CommonData Common;
    public CurrencyData Currency;
    public LevelData Level;

    public GameData()
    {
        Audio = new AudioData();
        Common = new CommonData();
        Currency = new CurrencyData();
        Level = new LevelData();
    }
}
