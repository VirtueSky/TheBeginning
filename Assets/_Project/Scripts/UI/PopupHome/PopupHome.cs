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
        popupVariable?.Value.Show<PopupSetting>(false);
    }

    public void OnClickDailyReward()
    {
        popupVariable?.Value.Show<PopupDailyReward>(false);
    }

    public void OnClickShop()
    {
        popupVariable?.Value.Show<PopupShop>(false);
    }

    public void OnClickTest()
    {
        popupVariable?.Value.Show<PopupTest>(false);
    }

    public void OnClickOpenAdministrator()
    {
        popupVariable.Value.Show<PopupAdministrator>(false);
    }
}