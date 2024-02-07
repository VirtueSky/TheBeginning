using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupHome : UIPopup
{
    [SerializeField] private StringEvent changeSceneEvent;
    [SerializeField] private GameObject buttonAdmin;
    [SerializeField] private Vector3Variable posInOutPopupAdmin;
    [SerializeField] private GameConfig gameConfig;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        buttonAdmin.SetActive(gameConfig.enableAdministrator);
    }

    public void OnClickStartGame()
    {
        changeSceneEvent.Raise(Constant.GAME_SCENE);
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

    public void OnClickOpenAdministrator()
    {
        posInOutPopupAdmin.Value = buttonAdmin.transform.position;
        AppControlPopup.Show<PopupAdministrator>(false);
    }
}