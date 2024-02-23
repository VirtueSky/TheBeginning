using TheBeginning.AppControl;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;
using VirtueSky.Events;

public class PopupHome : UIPopup
{
    [SerializeField] private EventAudioHandle playMusicEvent;
    [SerializeField] private SoundData musicHome;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        playMusicEvent.Raise(musicHome);
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