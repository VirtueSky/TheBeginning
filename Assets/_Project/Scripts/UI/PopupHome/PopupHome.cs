using TheBeginning.AppControl;
using TheBeginning.UserData;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;
using VirtueSky.Events;

public class PopupHome : UIPopup
{
    [SerializeField] private PlayMusicEvent playMusicEvent;
    [SerializeField] private SoundData musicHome;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
    [SerializeField] private GameObject noticeDailyReward;
    [SerializeField] private EventNoParam claimDailyRewardEvent;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        claimDailyRewardEvent.AddListener(SetupNoticeDailyReward);
        playMusicEvent.Raise(musicHome);
        SetupNoticeDailyReward();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
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
}