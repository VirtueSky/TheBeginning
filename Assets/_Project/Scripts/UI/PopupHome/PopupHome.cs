using System;
using PrimeTween;
using TheBeginning.AppControl;
using TheBeginning.UserData;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupHome : UIPopup
{
    [SerializeField] private PlayMusicEvent playMusicEvent;
    [SerializeField] private SoundData musicHome;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
    [SerializeField] private GameObject noticeDailyReward;
    [SerializeField] private EventNoParam claimDailyRewardEvent;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private StringVariable versionUpdateVariable;
    [SerializeField] private BooleanVariable dontShowAgainPopupUpdate;
    [SerializeField] private StringEvent showNotificationInGameEvent;
    private Tween tween;

    private void Start()
    {
        App.Delay(1.0f, () => { showNotificationInGameEvent.Raise("Welcome TheBeginning"); });
    }

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        claimDailyRewardEvent.AddListener(SetupNoticeDailyReward);
        playMusicEvent.Raise(musicHome);
        SetupNoticeDailyReward();
    }

    protected override void OnAfterShow()
    {
        base.OnAfterShow();
        ShowPopupUpdate();
    }


    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        tween.Stop();
        claimDailyRewardEvent.RemoveListener(SetupNoticeDailyReward);
    }

    void SetupNoticeDailyReward()
    {
        noticeDailyReward.SetActive(!UserData.IsClaimedTodayDailyReward());
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

    void ShowPopupUpdate()
    {
        if (gameConfig.enableShowPopupUpdate && !dontShowAgainPopupUpdate.Value)
        {
            tween = Tween.Delay(0.5f, () =>
            {
                if (!versionUpdateVariable.Value.Equals(Application.version))
                {
                    AppControlPopup.Show<PopupUpdate>(false);
                }
            });
        }
    }
}