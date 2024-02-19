using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupHome : UIPopup
{
    [SerializeField] private Vector3Variable posInOutPopupAdmin;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
    }

    public void OnClickStartGame()
    {
        callPlayCurrentLevelEvent.Raise();
    }

    public void OnClickSetting()
    {
        AppControlPopup.Show<PopupSetting>(false);
    }

    public void OnClickDailyReward()
    {
        AppControlPopup.Show<PopupDailyReward>(false);
    }

    public void OnClickShop()
    {
        AppControlPopup.Show<PopupShop>(false);
    }

    public void OnClickTest()
    {
        AppControlPopup.Show<PopupTest>(false);
    }
}