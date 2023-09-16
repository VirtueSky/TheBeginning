using System.Reflection;


public class PopupHome : Popup
{
 

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        // PopupController.Instance.Show<PopupUI>();
      //  eventShowPopup.Raise(typeof(PopupUI));
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        // PopupController.Instance.Hide<PopupUI>();
       // eventHidePopup.Raise(typeof(PopupUI));
    }

    public void OnClickStart()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        GameManager.Instance.StartGame();
    }

    public void OnClickDebug()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);
       // eventShowPopup.Raise(typeof(PopupDebug));
        // PopupController.Instance.Show<PopupDebug>();
    }

    public void OnClickSetting()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        //PopupController.Instance.Show<PopupSetting>();
       // eventShowPopup.Raise(typeof(PopupSetting));
    }

    public void OnClickDailyReward()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        // PopupController.Instance.Show<PopupDailyReward>();
      //  eventShowPopup.Raise(typeof(PopupDailyReward));
    }

    public void OnClickShop()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        // PopupController.Instance.Show<PopupShop>();
      //  eventShowPopup.Raise(typeof(PopupShop));
    }

    public void OnClickTest()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        // PopupController.Instance.Show<PopupTest>();
    }
}