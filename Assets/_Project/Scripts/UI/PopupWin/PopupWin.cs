using System.Reflection;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TheBeginning.Config;
using TheBeginning.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
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

        [SerializeField] private EventNoParam moveAllCoinDone;

        [HeaderLine(Constant.SO_Variable)] [SerializeField]
        private RewardAdVariable rewardAdVariable;

        private float percent = 0;
        private bool waitMoveAllCoinDone;

        public int MoneyWin => GameConfig.Instance.winLevelMoney;


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
            UserData.PercentWinGift += GameConfig.Instance.percentWinGiftPerLevel;
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

        private void Setup()
        {
            BtnRewardAds.SetActive(true);
            BtnTapToContinue.SetActive(false);
        }

        public void OnClickAdsReward()
        {
            if (rewardAdVariable.AdUnitRewardVariable.IsReady()) BonusArrowHandler.MoveObject.StopMoving();
            rewardAdVariable.Show(GetRewardAds, () =>
                {
                    BonusArrowHandler.MoveObject.ResumeMoving();
                    BtnRewardAds.SetActive(true);
                    BtnTapToContinue.SetActive(true);
                }, trackingRewardPosition: $"{MethodBase.GetCurrentMethod().Name}_{this.name}");
        }

        private async void GetRewardAds()
        {
            CoinSystem.AddCoin(MoneyWin * BonusArrowHandler.CurrentAreaItem.MultiBonus,
                BtnRewardAds.transform.position);
            BonusArrowHandler.MoveObject.StopMoving();
            BtnRewardAds.SetActive(false);
            BtnTapToContinue.SetActive(false);
            await UniTask.WaitUntil(() => waitMoveAllCoinDone);
            Hide();
            playCurrentLevelEvent.Raise();
        }

        public async void OnClickContinue()
        {
            CoinSystem.AddCoin(MoneyWin, BtnTapToContinue.transform.position);
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
}