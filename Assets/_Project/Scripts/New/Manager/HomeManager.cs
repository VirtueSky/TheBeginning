using UnityEngine;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private LoadSceneEvent loadSceneEvent;
    [SerializeField] private PopupVariable popupVariable;

    public void LoadGameScene()
    {
        loadSceneEvent.Raise(new LoadSceneData(false, Constant.GAMEPLAY_SCENE, .1f, null));
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
}