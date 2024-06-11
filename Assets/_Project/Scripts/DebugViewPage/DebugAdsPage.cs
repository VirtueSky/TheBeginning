using System.Collections;
using System.Threading.Tasks;
using UnityDebugSheet.Runtime.Core.Scripts;
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
        protected override string Title => "Ads Debug";

        public void Init(InterAdVariable _interAdVariable, RewardAdVariable _rewardAdVariable,
            BannerAdVariable _bannerAdVariable, BooleanVariable _offInter, BooleanVariable _offBanner,
            BooleanVariable _offReward)
        {
            interAdVariable = _interAdVariable;
            rewardAdVariable = _rewardAdVariable;
            bannerAdVariable = _bannerAdVariable;
            offInterVariable = _offInter;
            offBannerVariable = _offBanner;
            offRewardVariable = _offReward;
        }

        public override Task Initialize()
        {
            AddButton("Show Banner", clicked: () => bannerAdVariable.AdUnitBannerVariable.Show());
            AddButton("Hide Banner", clicked: () => bannerAdVariable.Hide());
            AddButton("Show Inter", clicked: () => interAdVariable.AdUnitInterVariable.Show());
            AddButton("Show Reward", clicked: () => rewardAdVariable.AdUnitRewardVariable.Show());
            AddSwitch(offInterVariable.Value, "Is Off Inter", valueChanged: b => offInterVariable.Value = b,
                icon: DebugViewStatic.IconToggleDebug);
            AddSwitch(offBannerVariable.Value, "Is Off Banner", valueChanged: b => offBannerVariable.Value = b,
                icon: DebugViewStatic.IconToggleDebug);
            AddSwitch(offRewardVariable.Value, "Is Off Reward", valueChanged: b => offRewardVariable.Value = b,
                icon: DebugViewStatic.IconToggleDebug);
            return base.Initialize();
        }
    }
}