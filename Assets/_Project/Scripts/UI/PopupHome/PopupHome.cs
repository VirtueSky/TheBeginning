using System;
using TheBeginning.Config;
using TheBeginning.Data;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Tweening;
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

        public void OnClickLeaderboard()
        {
            PopupManager.Show<PopupLeaderboard>(false);
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
            if (gameConfig.EnableShowPopupUpdate && !dontShowAgainPopupUpdate.Value)
            {
                Tween.Delay(0.5f, () =>
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