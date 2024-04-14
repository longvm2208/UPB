public class PopupLose : PopupBase
{
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
