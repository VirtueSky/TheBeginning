using System;
using TheBeginning.UserData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Variables;

public class DailyRewardItem : MonoBehaviour
{
    [ReadOnly] public int dayIndex;
    public TextMeshProUGUI textDay;
    public TextMeshProUGUI textValue;

    public Image greenTick;

    //public Image backgroundClaim;
    public Image backgroundCanNotClaim;
    public Image iconLock;
    public Image iconItem;
    [SerializeField] private IntegerVariable currentCoin;

    [SerializeField] private Vector3Event generateCoinEvent;
    [SerializeField] private DailyRewardConfig dailyRewardConfig;

    //[SerializeField] private EventNoParam claimRewardEvent;
    private int coinValue;
    private DailyRewardItemState dailyRewardItemState;
    private DailyRewardData dailyRewardData;
    public DailyRewardItemState DailyRewardItemState => dailyRewardItemState;

    public DailyRewardData DailyRewardData => dailyRewardData;

    public void SetUp(int i)
    {
        dayIndex = i + 1;
        SetUpData();
        SetUpUI(i);
    }

    public void SetDefaultUI()
    {
        iconLock.gameObject.SetActive(false);
        //backgroundClaim.gameObject.SetActive(false);
        backgroundCanNotClaim.gameObject.SetActive(false);
        greenTick.gameObject.SetActive(false);
    }

    private void SetUpData()
    {
        // Setup data
        dailyRewardData = UserData.IsStartLoopingDailyReward
            ? dailyRewardConfig.DailyRewardDatasLoop[dayIndex - 1]
            : dailyRewardConfig.DailyRewardDatas[dayIndex - 1];

        coinValue = dailyRewardData.value;
        // Setup states
        if (dailyRewardData.dailyRewardType == DailyRewardType.Currency)
        {
        }
        else if (dailyRewardData.dailyRewardType == DailyRewardType.Skin)
        {
            //shopItemData = ConfigController.ItemConfig.GetShopItemDataById(dailyRewardData.SkinID);
        }

        if (UserData.DailyRewardDayIndex > dayIndex)
        {
            dailyRewardItemState = DailyRewardItemState.Claimed;
        }
        else if (UserData.DailyRewardDayIndex == dayIndex)
        {
            if (!UserData.IsClaimedTodayDailyReward())
            {
                dailyRewardItemState = DailyRewardItemState.ReadyToClaim;
            }
            else
            {
                dailyRewardItemState = DailyRewardItemState.NotClaim;
            }
        }
        else
        {
            dailyRewardItemState = DailyRewardItemState.NotClaim;
        }
    }

    public void SetUpUI(int i)
    {
        SetDefaultUI();
        textDay.text = "Day " + (i + 1);
        textValue.text = coinValue.ToString();
        switch (dailyRewardItemState)
        {
            case DailyRewardItemState.Claimed:
                //backgroundClaim.gameObject.SetActive(false);
                backgroundCanNotClaim.gameObject.SetActive(true);
                greenTick.gameObject.SetActive(true);
                break;
            case DailyRewardItemState.ReadyToClaim:
                //backgroundClaim.gameObject.SetActive(true);
                backgroundCanNotClaim.gameObject.SetActive(false);
                greenTick.gameObject.SetActive(false);
                break;
            case DailyRewardItemState.NotClaim:
                iconLock.gameObject.SetActive(true);
                //backgroundClaim.gameObject.SetActive(false);
                backgroundCanNotClaim.gameObject.SetActive(false);
                greenTick.gameObject.SetActive(false);
                break;
        }

        switch (dailyRewardData.dailyRewardType)
        {
            case DailyRewardType.Currency:
                textValue.gameObject.SetActive(true);
                iconItem.sprite = dailyRewardData.icon;
                iconItem.SetNativeSize();
                break;
            case DailyRewardType.Skin:
                //Icon.sprite = shopItemData.Icon;
                iconItem.SetNativeSize();
                break;
        }
    }

    public void OnClaim(bool isClaimX5 = false, Action claimCompleted = null)
    {
        // Save datas
        UserData.LastDailyRewardClaimed = DateTime.Now.ToString();
        UserData.DailyRewardDayIndex++;
        UserData.TotalClaimDailyReward++;

        // Claim by type
        switch (dailyRewardData.dailyRewardType)
        {
            case DailyRewardType.Currency:
                generateCoinEvent.Raise(this.gameObject.transform.position);
                currentCoin.Value += coinValue * (isClaimX5 ? 5 : 1);
                break;
            case DailyRewardType.Skin:
                //shopItemData.IsUnlocked = true;
                //Data.CurrentEquippedSkin = shopItemData.Id;
                break;
        }

        claimCompleted?.Invoke();
    }
}

public enum DailyRewardItemState
{
    Claimed,
    ReadyToClaim,
    NotClaim
}