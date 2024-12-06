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
    public class HomePopup : UIPopup
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private PlayMusicEvent playMusicEvent;
        [SerializeField] private SoundData musicHome;
        [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
        [SerializeField] private GameObject noticeDailyReward;
        [SerializeField] private EventNoParam claimDailyRewardEvent;
        [SerializeField] private StringVariable versionUpdateVariable;
        [SerializeField] private BooleanVariable dontShowAgainPopupUpdate;

        private Tween tween;

        private void Start()
        {
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
            PopupManager.Show<SettingPopup>(false);
        }

        public void OnClickDailyReward()
        {
            PopupManager.Show<DailyRewardPopup>(false);
        }

        public void OnClickLeaderboard()
        {
            PopupManager.Show<LeaderboardPopup>(false);
        }

        void ShowPopupUpdate()
        {
            if (gameSettings.EnableShowPopupUpdate && !dontShowAgainPopupUpdate.Value)
            {
                tween = Tween.Delay(0.5f, () =>
                {
                    if (!versionUpdateVariable.Value.Equals(Application.version))
                    {
                        PopupManager.Show<UpdatePopup>(false);
                    }
                });
            }
        }
    }
}