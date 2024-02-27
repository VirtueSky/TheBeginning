using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PrimeTween;
using TheBeginning.AppControl;
using TheBeginning.UserData;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupDailyReward : UIPopup
{
    [TitleColor("Attribute", CustomColor.Lavender, CustomColor.Cornsilk)]
    public GameObject BtnWatchVideo;

    public GameObject BtnClaim;
    [SerializeField] private EventNoParam claimRewardEvent;
    [ReadOnly] public DailyRewardItem CurrentItem;
    public List<DailyRewardItem> DailyRewardItems => GetComponentsInChildren<DailyRewardItem>().ToList();

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        claimRewardEvent.AddListener(Setup);
        ResetDailyReward();
        Setup();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        claimRewardEvent.RemoveListener(Setup);
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

        if (CurrentItem)
        {
            if (CurrentItem.DailyRewardItemState == DailyRewardItemState.ReadyToClaim)
            {
                BtnWatchVideo.SetActive(CurrentItem.DailyRewardData.DailyRewardType == DailyRewardType.Currency);
                BtnClaim.SetActive(true);
            }
            else
            {
                BtnWatchVideo.SetActive(false);
                BtnClaim.SetActive(false);
            }
        }
        else
        {
            BtnWatchVideo.SetActive(false);
            BtnClaim.SetActive(false);
        }
    }

    public void OnClickBtnClaimX5Video()
    {
        AppControlAds.ShowReward(() => { CurrentItem.OnClaim(true, () => claimRewardEvent.Raise()); });
    }

    public void OnClickBtnClaim()
    {
        CurrentItem.OnClaim(false, () => claimRewardEvent.Raise());
    }

    public void OnClickNextDay()
    {
        UserData.LastDailyRewardClaimed = DateTime.Now.AddDays(-1).ToString();
        ResetDailyReward();
        Setup();
    }
}