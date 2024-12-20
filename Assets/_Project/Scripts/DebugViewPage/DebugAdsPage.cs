using System.Threading.Tasks;
using System.Collections;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugAdsPage : DefaultDebugPageBase
    {
        private InterAdVariable interAdVariable;
        private BannerAdVariable bannerAdVariable;
        private RewardAdVariable rewardAdVariable;
        private BooleanVariable debugOnOffInterVariable;
        private BooleanVariable debugOnOffBannerVariable;
        private BooleanVariable debugOnOffRewardVariable;
        private Sprite iconToggle;
        private StringEvent showNotificationInGameEvent;
        protected override string Title => "Ads Debug";

        public void Init(InterAdVariable _interAdVariable, RewardAdVariable _rewardAdVariable,
            BannerAdVariable _bannerAdVariable, BooleanVariable _onOffInter, BooleanVariable _onOffBanner,
            BooleanVariable _onOffReward, Sprite _iconToggle, StringEvent _showNotiEvent)
        {
            interAdVariable = _interAdVariable;
            rewardAdVariable = _rewardAdVariable;
            bannerAdVariable = _bannerAdVariable;
            debugOnOffInterVariable = _onOffInter;
            debugOnOffBannerVariable = _onOffBanner;
            debugOnOffRewardVariable = _onOffReward;
            iconToggle = _iconToggle;
            showNotificationInGameEvent = _showNotiEvent;
        }


#if UDS_USE_ASYNC_METHODS
        public override Task Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#else
        public override IEnumerator Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#endif
        void OnInitialize()
        {
            AddButton("Show Banner", clicked: ShowBanner);
            AddButton("Hide Banner", clicked: HideBanner);
            AddButton("Show Inter", clicked: ShowInter);
            AddButton("Show Reward", clicked: ShowReward);
            AddSwitch(debugOnOffInterVariable.Value, "On/Off Inter", valueChanged: b => debugOnOffInterVariable.Value = b,
                icon: iconToggle);
            AddSwitch(debugOnOffBannerVariable.Value, "On/Off Banner", valueChanged: b => debugOnOffBannerVariable.Value = b,
                icon: iconToggle);
            AddSwitch(debugOnOffRewardVariable.Value, "On/Off Reward", valueChanged: b => debugOnOffRewardVariable.Value = b,
                icon: iconToggle);
        }

        void ShowBanner()
        {
            if (Application.isMobilePlatform)
            {
                bannerAdVariable.AdUnitBannerVariable.Show();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works on mobile platform");
            }
        }

        void HideBanner()
        {
            if (Application.isMobilePlatform)
            {
                bannerAdVariable.Hide();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works on mobile platform");
            }
        }

        void ShowInter()
        {
            if (Application.isMobilePlatform)
            {
                interAdVariable.AdUnitInterVariable.Show();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works on mobile platform");
            }
        }

        void ShowReward()
        {
            if (Application.isMobilePlatform)
            {
                rewardAdVariable.AdUnitRewardVariable.Show().OnCompleted(() => Debug.Log("Reward Completed..."));
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works on mobile platform");
            }
        }
    }
}