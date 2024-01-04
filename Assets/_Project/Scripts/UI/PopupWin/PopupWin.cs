using DG.Tweening;
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

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventNoParam playCurrentLevelEvent;

    [SerializeField] private Vector3Event generateCoinEvent;
    [SerializeField] private EventNoParam claimRewardEvent;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable currencyTotalVariable;

    [SerializeField] private AdManagerVariable adManagerVariable;
    [SerializeField] private BooleanVariable isTestingVariable;


    private float percent = 0;
    private Sequence sequence;
    public int MoneyWin => Config.Game.WinLevelMoney;


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
        Data.PercentWinGift += Config.Game.PercentWinGiftPerLevel;
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
        if (isTestingVariable.Value)
        {
            GetRewardAds();
            claimRewardEvent.Raise();
        }
        else
        {
            if (adManagerVariable.Value.IsRewardReady()) BonusArrowHandler.MoveObject.StopMoving();
            adManagerVariable.Value.ShowRewardAds(() => { GetRewardAds(); }, null, null, () =>
            {
                BonusArrowHandler.MoveObject.ResumeMoving();
                BtnRewardAds.SetActive(true);
                BtnTapToContinue.SetActive(true);
            });
        }
    }

    public void GetRewardAds()
    {
        generateCoinEvent.Raise(BtnRewardAds.transform.position);
        currencyTotalVariable.Value += MoneyWin * BonusArrowHandler.CurrentAreaItem.MultiBonus;
        BonusArrowHandler.MoveObject.StopMoving();
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        sequence?.Kill();
        claimRewardEvent.Raise();
        DOTween.Sequence().AppendInterval(1.2f).AppendCallback(() =>
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

        DOTween.Sequence().AppendInterval(1.2f).AppendCallback(() =>
        {
            playCurrentLevelEvent.Raise();
            Hide();
        });
    }

    private void ReceiveGift()
    {
    }
}