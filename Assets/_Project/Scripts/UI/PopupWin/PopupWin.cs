using PrimeTween;
using TheBeginning.AppControl;
using TheBeginning.UserData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupWin : UIPopup
{
    [HeaderLine(Constant.Normal_Attribute)]
    public BonusArrowHandler BonusArrowHandler;

    public GameObject BtnRewardAds;
    public GameObject BtnTapToContinue;
    [ReadOnly] public int TotalMoney;
    public Image ProcessBar;
    public TextMeshProUGUI TextPercentGift;
    [SerializeField] private AudioClip soundPopupWin;
    [SerializeField] private GameConfig gameConfig;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventNoParam playCurrentLevelEvent;

    [SerializeField] private Vector3Event generateCoinEvent;
    [SerializeField] private EventNoParam claimRewardEvent;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable currencyTotalVariable;

    private float percent = 0;

    //   private Tween tween;
    public int MoneyWin => gameConfig.WinLevelMoney;


    public float Percent
    {
        get => percent;
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            percent = value;
            ProcessBar.DOFillAmount(percent / 100, .5f).OnUpdate(ProcessBar,
                    (image, tween) => { TextPercentGift.text = ((int)(ProcessBar.fillAmount * 100 + 0.1f) + "%"); })
                .OnComplete(() =>
                {
                    if (percent >= 100)
                    {
                        ReceiveGift();
                    }
                });
        }
    }

    public void ClearProgress()
    {
        ProcessBar.DOFillAmount(0, 1f)
            .OnUpdate(ProcessBar,
                (image, tween) => { TextPercentGift.text = ((int)(ProcessBar.fillAmount * 100)) + "%"; });
    }

    private void SetupProgressBar()
    {
        ProcessBar.fillAmount = (float)UserData.PercentWinGift / 100;
        UserData.PercentWinGift += gameConfig.PercentWinGiftPerLevel;
        Percent = (float)UserData.PercentWinGift;
        if (UserData.PercentWinGift == 100)
        {
            UserData.PercentWinGift = 0;
        }
    }

    public void SetupMoneyWin(int bonusMoney)
    {
        TotalMoney = MoneyWin + bonusMoney;
    }

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        Setup();
        SetupProgressBar();
        Tween.Delay(2f, () => { BtnTapToContinue.SetActive(true); });
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
        if (AppControlAds.IsRewardReady()) BonusArrowHandler.MoveObject.StopMoving();
        AppControlAds.ShowReward(() => { GetRewardAds(); }, null, null, () =>
        {
            BonusArrowHandler.MoveObject.ResumeMoving();
            BtnRewardAds.SetActive(true);
            BtnTapToContinue.SetActive(true);
        });
    }

    public void GetRewardAds()
    {
        generateCoinEvent.Raise(BtnRewardAds.transform.position);
        currencyTotalVariable.Value += MoneyWin * BonusArrowHandler.CurrentAreaItem.MultiBonus;
        BonusArrowHandler.MoveObject.StopMoving();
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        claimRewardEvent.Raise();
        Tween.Delay(1.2f, () =>
        {
            Hide();
            playCurrentLevelEvent.Raise();
        });
    }

    public void OnClickContinue()
    {
        generateCoinEvent.Raise(BtnTapToContinue.transform.position);
        currencyTotalVariable.Value += MoneyWin;
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        Tween.Delay(1.2f, () =>
        {
            playCurrentLevelEvent.Raise();
            Hide();
        });
    }

    private void ReceiveGift()
    {
    }
}