using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using PrimeTween;
using TheBeginning.Config;
using TheBeginning.Data;
using UnityEngine.Serialization;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Utils;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    public class DailyRewardPopup : UIPopup
    {
        [TitleColor("Attribute", CustomColor.Lavender, CustomColor.Cornsilk)]
        public GameObject BtnWatchVideo;

        public GameObject BtnClaim;
        [SerializeField] private EventNoParam claimRewardEvent;

        [FormerlySerializedAs("rewardVariable")] [SerializeField]
        private RewardAdVariable rewardAdVariable;

        [ReadOnly] public DailyRewardItem CurrentItem;
        public List<DailyRewardItem> DailyRewardItems => GetComponentsInChildren<DailyRewardItem>().ToList();

        protected override void OnBeforeShow()
        {
            base.OnBeforeShow();
            ResetDailyReward();
            Setup();
        }

        public void ResetDailyReward()
        {
            if (!UserData.IsClaimedTodayDailyReward() && UserData.DailyRewardDayIndex == 29)
            {
                UserData.DailyRewardDayIndex = 1;
                UserData.IsStartLoopingDailyReward = true;
            }
        }

        private bool IsCurrentItem(int index)
        {
            return UserData.DailyRewardDayIndex == index;
        }

        public void Setup()
        {
            SetUpItems();
        }

        private void SetUpItems()
        {
            var week = (UserData.DailyRewardDayIndex - 1) / 7;
            if (UserData.IsClaimedTodayDailyReward()) week = (UserData.DailyRewardDayIndex - 2) / 7;

            for (var i = 0; i < 7; i++)
            {
                var item = DailyRewardItems[i];
                item.SetUp(i + 7 * week);
                if (IsCurrentItem(item.dayIndex)) CurrentItem = item;
            }

            BtnWatchVideo.SetActive(false);
            BtnClaim.SetActive(false);
            if (CurrentItem && CurrentItem.DailyRewardItemState == DailyRewardItemState.ReadyToClaim)
            {
                BtnWatchVideo.SetActive(CurrentItem.DailyRewardData.dailyRewardType == DailyRewardType.Coin);
                BtnClaim.SetActive(true);
            }
        }

        public void OnClickBtnClaimX5Video()
        {
            rewardAdVariable.Show(() =>
                {
                    CurrentItem.OnClaim(true, () =>
                    {
                        claimRewardEvent.Raise();
                        Setup();
                    });
                }, trackingRewardPosition: $"{MethodBase.GetCurrentMethod().Name}_{this.name}");
        }

        public void OnClickBtnClaim()
        {
            CurrentItem.OnClaim(false, () =>
            {
                claimRewardEvent.Raise();
                Setup();
            });
        }

        public void OnClickNextDay()
        {
            UserData.LastDailyRewardClaimed = DateTime.Now.AddDays(-1).ToString();
            ResetDailyReward();
            Setup();
        }
    }
}