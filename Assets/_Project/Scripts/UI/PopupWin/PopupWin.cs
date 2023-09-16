using DG.Tweening;

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : Popup
{
    public BonusArrowHandler BonusArrowHandler;
    public GameObject BtnRewardAds;
    public GameObject BtnTapToContinue;
    [ReadOnly] public int TotalMoney;
    public Image ProcessBar;
    public TextMeshProUGUI TextPercentGift;
    private float percent = 0;
    private Sequence sequence;
    public int MoneyWin => ConfigController.Game.WinLevelMoney;


    public float Percent
    {
        get => percent;
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            percent = value;
            ProcessBar.DOFillAmount(percent / 100, 0.5f).OnUpdate((() => { TextPercentGift.text = ((int)(ProcessBar.fillAmount * 100 + 0.1f)) + "%"; })).OnComplete((() =>
            {
                if (percent >= 100)
                {
                    ReceiveGift();
                }
            }));
        }
    }

    public void ClearProgress()
    {
        ProcessBar.DOFillAmount(0, 1f)
            .OnUpdate(() => { TextPercentGift.text = ((int)(ProcessBar.fillAmount * 100)) + "%"; });
    }

    private void SetupProgressBar()
    {
        ProcessBar.fillAmount = (float)Data.PercentWinGift / 100;
        Data.PercentWinGift += ConfigController.Game.PercentWinGiftPerLevel;
        Percent = (float)Data.PercentWinGift;
        if (Data.PercentWinGift == 100)
        {
            Data.PercentWinGift = 0;
        }
    }

    public void SetupMoneyWin(int bonusMoney)
    {
        TotalMoney = MoneyWin + bonusMoney;
    }

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        //  PopupController.Instance.Show<PopupUI>();
        Setup();
        SetupProgressBar();
        sequence = DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { BtnTapToContinue.SetActive(true); });
    }


    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        //  PopupController.Instance.Hide<PopupUI>();
    }

    public void Setup()
    {
        BtnRewardAds.SetActive(true);
        BtnTapToContinue.SetActive(false);
    }

    public void OnClickAdsReward()
    {
        if (Data.IsTesting)
        {
            GetRewardAds();
            Observer.ClaimReward?.Invoke();
        }
        else
        {
            // if (rewardVariable.IsReady()) BonusArrowHandler.MoveObject.StopMoving();
            // eventShowRewardAd.Raise(new ShowRewardAdData(() =>
            // {
            //     GetRewardAds();
            //     Observer.ClaimReward?.Invoke();
            // }, null, null, () =>
            // {
            //     BonusArrowHandler.MoveObject.ResumeMoving();
            //     BtnRewardAds.SetActive(true);
            //     BtnTapToContinue.SetActive(true);
            // }));
        }
    }

    public void GetRewardAds()
    {
        Data.CurrencyTotal += TotalMoney * BonusArrowHandler.CurrentAreaItem.MultiBonus;
        BonusArrowHandler.MoveObject.StopMoving();
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        sequence?.Kill();

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }

    public void OnClickContinue()
    {
        Data.CurrencyTotal += TotalMoney;
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }

    private void ReceiveGift()
    {
    }
}