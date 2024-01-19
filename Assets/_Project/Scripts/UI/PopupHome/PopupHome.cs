using UnityEngine;
using VirtueSky.Events;

public class PopupHome : UIPopup
{
    [SerializeField] private StringEvent changeSceneEvent;
    [SerializeField] private PopupVariable popupVariable;

    public void OnClickStartGame()
    {
        changeSceneEvent.Raise(Constant.GAME_SCENE);
    }

    public void OnClickSetting()
    {
        popupVariable?.Value.Show<PopupSetting>();
    }

    public void OnClickDebug()
    {
        popupVariable?.Value.Show<PopupDebug>();
    }

    public void OnClickDailyReward()
    {
        popupVariable?.Value.Show<PopupDailyReward>();
    }

    public void OnClickShop()
    {
        popupVariable?.Value.Show<PopupShop>();
    }

    public void OnClickTest()
    {
        popupVariable?.Value.Show<PopupTest>();
    }
}