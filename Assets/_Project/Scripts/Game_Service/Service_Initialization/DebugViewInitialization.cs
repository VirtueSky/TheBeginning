using TheBeginning.DebugViewPage;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class DebugViewInitialization : ServiceInitialization
    {
        [SerializeField] private DebugSheet debugViewSheet;
        [SerializeField] private GameConfig gameConfig;
        [HeaderLine("Tool")] [SerializeField] private IntegerVariable currentCoin;
        [SerializeField] private ItemConfig itemConfig;
        [SerializeField] private BooleanVariable isOffUiVariable;
        [HeaderLine("Ads"), SerializeField] private InterAdVariable interAdVariable;
        [SerializeField] private BannerAdVariable bannerAdVariable;
        [SerializeField] private RewardAdVariable rewardAdVariable;
        [SerializeField] private BooleanVariable offInterDebugVariable;
        [SerializeField] private BooleanVariable offBannerDebugVariable;
        [SerializeField] private BooleanVariable offRewardDebugVariable;
        [HeaderLine("Level"), SerializeField] private EventNoParam callPlayCurrentLevelEvent;
        [SerializeField] private GameStateVariable gameStateVariable;
        [SerializeField] private EventNoParam callNextLevelEvent;
        [SerializeField] private EventNoParam callPreviousLevelEvent;
        [SerializeField] private FloatEvent callWinLevelEvent;
        [SerializeField] private FloatEvent callLoseLevelEvent;
        [SerializeField] private IntegerVariable indexLevel;
        [SerializeField] private StringEvent showNotificationInGameEvent;

        public override void Initialization()
        {
            if (!gameConfig.enableAdministrator) return;
            debugViewSheet.gameObject.SetActive(true);
            var initialPage = debugViewSheet.GetOrCreateInitialPage("TheBeginning Debug");
            // add game page
            initialPage.AddPageLinkButton<DebugGamePage>("Tool Debug",
                icon: DebugViewStatic.CreateIconDebug(DebugViewStatic.ICON_TOOL_DEBUG),
                onLoad: debugView => { debugView.page.Init(currentCoin, itemConfig, isOffUiVariable); });

            // add ads page
            initialPage.AddPageLinkButton<DebugAdsPage>("Ads Debug",
                icon: DebugViewStatic.CreateIconDebug(DebugViewStatic.ICON_ADS_DEBUG),
                onLoad: debugView =>
                {
                    debugView.page.Init(interAdVariable, rewardAdVariable, bannerAdVariable, offInterDebugVariable,
                        offBannerDebugVariable, offRewardDebugVariable);
                });

            // add Level page
            initialPage.AddPageLinkButton<DebugLevelPage>("Level Debug",
                icon: DebugViewStatic.CreateIconDebug(DebugViewStatic.ICON_LEVEL_DEBUG),
                onLoad: debugView =>
                {
                    debugView.page.Init(gameStateVariable, callPlayCurrentLevelEvent, callNextLevelEvent,
                        callPreviousLevelEvent, callWinLevelEvent, callLoseLevelEvent, indexLevel,
                        showNotificationInGameEvent);
                });
            initialPage.Reload();
        }
    }
}