using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Ads;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    public class PopupTest : UIPopup
    {
        public TextMeshProUGUI textNotice;

        [Header("Unit variables")] public AdUnitVariable banner;
        public AdUnitVariable inter;
        public AdUnitVariable reward;

        public void ShowBanner()
        {
            LogMessage("Show Banner");
            // adManagerVariable.Value.ShowBanner();
            banner.Show();
        }

        public void HideBanner()
        {
            LogMessage("Hide Banner");
            // adManagerVariable.Value.HideBanner();
            banner.Destroy();
        }

        public void ShowRewards()
        {
            // adManagerVariable.Value.ShowRewardAds(() =>
            // {
            //     currencyVariable.Value += 100;
            //     LogMessage("Reward Completed");
            // }, null, null, () =>
            // {
            //     LogMessage("Skip reward");
            // });
            reward.Show().OnCompleted(() =>
            {
                CoinSystem.AddCoin(100);
                LogMessage("Reward Completed");
            }).OnSkipped(() => { LogMessage("Skip reward"); });
        }

        public void ShowInterAds()
        {
            // adManagerVariable.Value.ShowInterstitial(() =>
            // {
            //     LogMessage("Interstitial completed");
            // });
            inter.Show().OnCompleted(() => { LogMessage("Interstitial completed"); });
        }

        public void LogMessage(string message)
        {
            Debug.Log(message);
            textNotice.text = message;
        }
    }
}