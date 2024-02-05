using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupHome : UIPopup
{
    [SerializeField] private StringEvent changeSceneEvent;
    [SerializeField] private PopupVariable popupVariable;
    [SerializeField] private GameObject buttonAdmin;
    [SerializeField] private Vector3Variable posInOutPopupAdmin;

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
        posInOutPopupAdmin.Value = buttonAdmin.transform.position;
        popupVariable.Value.Show<PopupAdministrator>(false);
    }
}