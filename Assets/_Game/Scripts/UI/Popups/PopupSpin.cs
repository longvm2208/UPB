using UnityEngine;

public class PopupSpin : PopupBase
{
    [SerializeField]
    private FortuneWheel fortuneWheel;

    public override void Open(object args)
    {
        base.Open(args);
    }
    
    #region UI Events
    public void OnClickClose()
    {
        Close();
    }
    #endregion
}
