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
        [HeaderLine("Icon"), SerializeField] private Sprite iconTool;
        [SerializeField] private Sprite iconAds;
        [SerializeField] private Sprite iconLevel;
        [SerializeField] private Sprite iconWin;
        [SerializeField] private Sprite iconLose;
        [SerializeField] private Sprite iconNext;
        [SerializeField] private Sprite iconBack;
        [SerializeField] private Sprite iconToggle;
        [SerializeField] private Sprite iconInput;
        [SerializeField] private Sprite iconOke;
        [HeaderLine("Tool")] [SerializeField] private IntegerVariable currentCoin;
        [SerializeField] private ItemConfig itemConfig;
        [SerializeField] private BooleanVariable isOffUiVariable;
        [SerializeField] private BooleanVariable isTestingVariable;
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
            if (!gameConfig.enableDebugView)
            {
                debugViewSheet.gameObject.SetActive(false);
                return;
            }

            debugViewSheet.gameObject.SetActive(true);
            var initialPage = debugViewSheet.GetOrCreateInitialPage("TheBeginning Debug");
            // add game page
            initialPage.AddPageLinkButton<DebugGamePage>("Game Debug",
                icon: iconTool,
                onLoad: debugView =>
                {
                    debugView.page.Init(currentCoin, itemConfig, isOffUiVariable, isTestingVariable, iconInput, iconOke,
                        iconToggle);
                });

            // add ads page
            initialPage.AddPageLinkButton<DebugAdsPage>("Ads Debug",
                icon: iconAds,
                onLoad: debugView =>
                {
                    debugView.page.Init(interAdVariable, rewardAdVariable, bannerAdVariable, offInterDebugVariable,
                        offBannerDebugVariable, offRewardDebugVariable, iconToggle, showNotificationInGameEvent);
                });

            // add Level page
            initialPage.AddPageLinkButton<DebugLevelPage>("Level Debug",
                icon: iconLevel,
                onLoad: debugView =>
                {
                    debugView.page.Init(gameStateVariable, callPlayCurrentLevelEvent, callNextLevelEvent,
                        callPreviousLevelEvent, callWinLevelEvent, callLoseLevelEvent, indexLevel,
                        showNotificationInGameEvent, iconNext, iconBack, iconWin, iconLose, iconInput, iconOke);
                });
            initialPage.Reload();
        }
    }
}