using System.Reflection;

public class PopupLose : UIPopup
{
    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        //  PopupController.Instance.Show<PopupUI>();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        // PopupController.Instance.Hide<PopupUI>();
    }

    public void OnClickReplay()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        GameManager.Instance.ReplayGame();
    }
}