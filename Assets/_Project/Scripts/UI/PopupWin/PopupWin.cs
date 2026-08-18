using System.Reflection;
using TheBeginning.Currency;
using Cysharp.Threading.Tasks;
using TheBeginning.Config;
using TheBeginning.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Events;
using VirtueSky.Tweening;
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
        [SerializeField] private GameConfig gameConfig;

        [HeaderLine(Constant.SO_Event)] [SerializeField]
        private EventNoParam playCurrentLevelEvent;

        [SerializeField] private EventNoParam moveAllCoinDone;

        [HeaderLine(Constant.SO_Variable)] [SerializeField]
        [FormerlySerializedAs("rewardVariable")]
        private RewardAdVariable rewardAdVariable;

        [SerializeField] private CurrencyVariable coinCurrency;

        private float percent = 0;
        private bool waitMoveAllCoinDone;

        public int MoneyWin => gameConfig.WinLevelMoney;


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
                }, trackingRewardPosition: $"{MethodBase.GetCurrentMethod().Name}_{this.name}");
        }

        public async void GetRewardAds()
        {
            coinCurrency.Add(MoneyWin * BonusArrowHandler.CurrentAreaItem.MultiBonus,
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
            coinCurrency.Add(MoneyWin, BtnTapToContinue.transform.position);
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
