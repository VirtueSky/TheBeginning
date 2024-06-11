using PrimeTween;
using TheBeginning.AppControl;
using TheBeginning.UserData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Threading.Tasks;
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
    [SerializeField] private EventNoParam moveAllCoinDone;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable currentCoin;

    [FormerlySerializedAs("rewardVariable")] [SerializeField]
    private RewardAdVariable rewardAdVariable;

    private float percent = 0;
    private bool waitMoveAllCoinDone;

    public int MoneyWin => gameConfig.winLevelMoney;


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
        UserData.PercentWinGift += gameConfig.percentWinGiftPerLevel;
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
        waitMoveAllCoinDone = false;
        moveAllCoinDone.AddListener(OnMoveAllCoinDone);
        Setup();
        SetupProgressBar();
        Tween.Delay(2f, () => { BtnTapToContinue.SetActive(true); });
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        moveAllCoinDone.RemoveListener(OnMoveAllCoinDone);
    }

    public void Setup()
    {
        BtnRewardAds.SetActive(true);
        BtnTapToContinue.SetActive(false);
    }

    public void OnClickAdsReward()
    {
        if (rewardAdVariable.AdUnitRewardVariable.IsReady()) BonusArrowHandler.MoveObject.StopMoving();
        rewardAdVariable.Show(() => { GetRewardAds(); }, () =>
        {
            BonusArrowHandler.MoveObject.ResumeMoving();
            BtnRewardAds.SetActive(true);
            BtnTapToContinue.SetActive(true);
        });
    }

    public async void GetRewardAds()
    {
        generateCoinEvent.Raise(BtnRewardAds.transform.position);
        currentCoin.Value += MoneyWin * BonusArrowHandler.CurrentAreaItem.MultiBonus;
        BonusArrowHandler.MoveObject.StopMoving();
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        await UniTask.WaitUntil(() => waitMoveAllCoinDone);
        Hide();
        playCurrentLevelEvent.Raise();
    }

    public async void OnClickContinue()
    {
        generateCoinEvent.Raise(BtnTapToContinue.transform.position);
        currentCoin.Value += MoneyWin;
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        await UniTask.WaitUntil(() => waitMoveAllCoinDone);
        playCurrentLevelEvent.Raise();
        Hide();
    }

    private void ReceiveGift()
    {
    }

    void OnMoveAllCoinDone()
    {
        waitMoveAllCoinDone = true;
    }
}