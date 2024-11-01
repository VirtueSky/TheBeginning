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
        private BooleanVariable offInterVariable;
        private BooleanVariable offBannerVariable;
        private BooleanVariable offRewardVariable;
        private Sprite iconToggle;
        private StringEvent showNotificationInGameEvent;
        protected override string Title => "Ads Debug";

        public void Init(InterAdVariable _interAdVariable, RewardAdVariable _rewardAdVariable,
            BannerAdVariable _bannerAdVariable, BooleanVariable _offInter, BooleanVariable _offBanner,
            BooleanVariable _offReward, Sprite _iconToggle, StringEvent _showNotiEvent)
        {
            interAdVariable = _interAdVariable;
            rewardAdVariable = _rewardAdVariable;
            bannerAdVariable = _bannerAdVariable;
            offInterVariable = _offInter;
            offBannerVariable = _offBanner;
            offRewardVariable = _offReward;
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
            AddSwitch(offInterVariable.Value, "Is Off Inter", valueChanged: b => offInterVariable.Value = b,
                icon: iconToggle);
            AddSwitch(offBannerVariable.Value, "Is Off Banner", valueChanged: b => offBannerVariable.Value = b,
                icon: iconToggle);
            AddSwitch(offRewardVariable.Value, "Is Off Reward", valueChanged: b => offRewardVariable.Value = b,
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