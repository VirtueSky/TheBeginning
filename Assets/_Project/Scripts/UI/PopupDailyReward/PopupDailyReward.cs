using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PrimeTween;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupDailyReward : UIPopup
{
    public GameObject BtnWatchVideo;
    public GameObject BtnClaim;
    [SerializeField] private EventNoParam claimRewardEvent;
    [SerializeField] private BooleanVariable isTestingVariable;
    [SerializeField] private GameObject btnNextDay;
    [SerializeField] private AdManagerVariable adManagerVariable;
    [ReadOnly] public DailyRewardItem CurrentItem;
    public List<DailyRewardItem> DailyRewardItems => GetComponentsInChildren<DailyRewardItem>().ToList();

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        // PopupController.Instance.Show<PopupUI>();
        ResetDailyReward();
        Setup();
        btnNextDay.SetActive(isTestingVariable.Value);
    }

    public void ResetDailyReward()
    {
        if (!UserData.IsClaimedTodayDailyReward() && UserData.DailyRewardDayIndex == 29)
        {
            UserData.DailyRewardDayIndex = 1;
            UserData.IsStartLoopingDailyReward = true;
        }
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        //PopupController.Instance.HideAll();
        //PopupController.Instance.Show<PopupHome>();
        // if (!PopupController.Instance.Get<PopupHome>().isActiveAndEnabled)
        // {
        //     GameManager.Instance.gameState = GameState.PlayingGame;
        //     PopupController.Instance.Hide<PopupUI>();
        // }
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
            item.SetUp(this, i + 7 * week);
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
        adManagerVariable.Value.ShowRewardAds(() => { CurrentItem.OnClaim(true); });
    }

    public void OnClickBtnClaim()
    {
        CurrentItem.OnClaim();
    }

    public void OnClickNextDay()
    {
        UserData.LastDailyRewardClaimed = DateTime.Now.AddDays(-1).ToString();
        ResetDailyReward();
        Setup();
        //Observer.OnNotifying?.Invoke();
    }
}