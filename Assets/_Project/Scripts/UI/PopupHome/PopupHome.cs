using System;
using PrimeTween;
using TheBeginning.Config;
using TheBeginning.Data;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
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
            PopupManager.Show<PopupSetting>(false);
        }

        public void OnClickDailyReward()
        {
            PopupManager.Show<PopupDailyReward>(false);
        }

        public void OnClickShop()
        {
            PopupManager.Show<PopupShop>(false);
        }

        public void OnClickTest()
        {
            PopupManager.Show<PopupTest>(false);
        }

        void ShowPopupUpdate()
        {
            if (gameConfig.enableShowPopupUpdate && !dontShowAgainPopupUpdate.Value)
            {
                tween = Tween.Delay(0.5f, () =>
                {
                    if (!versionUpdateVariable.Value.Equals(Application.version))
                    {
                        PopupManager.Show<PopupUpdate>(false);
                    }
                });
            }
        }
    }
}