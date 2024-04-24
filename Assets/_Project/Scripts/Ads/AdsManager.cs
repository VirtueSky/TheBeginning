using System;
using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.FirebaseTracking;
using VirtueSky.Variables;

[EditorIcon("icon_manager"), HideMonoScript]
public class AdsManager : BaseMono
{
    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable adsCounterVariable;

    [SerializeField] private GameStateVariable gameStateVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private BooleanVariable isOffInterAdsVariable;
    [SerializeField] private BooleanVariable isOffBannerVariable;
    [SerializeField] private BooleanVariable isOffRewardVariable;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private StringEvent showNotificationInGameEvent;

    [Space] [HeaderLine("Firebase Remote Config")] [SerializeField]
    private IntegerVariable remoteConfigLevelTurnOnInterstitial;

    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingLevelVariable;
    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingTimeVariable;
    [SerializeField] private BooleanVariable remoteConfigOnOffInterstitial;
    [SerializeField] private BooleanVariable remoteConfigOnOffBanner;

    [Space] [HeaderLine("Log Event Firebase Analytic")] [SerializeField]
    private LogEventFirebaseNoParam logEventRequestInter;

    [SerializeField] private LogEventFirebaseNoParam logEventShowInterCompleted;
    [SerializeField] private LogEventFirebaseNoParam logEventRequestReward;
    [SerializeField] private LogEventFirebaseNoParam logEventShowRewardCompleted;
    [SerializeField] private LogEventFirebaseNoParam logEventShowBanner;
    [SerializeField] private LogEventFirebaseNoParam logEventHideBanner;

    [Space] [HeaderLine("Ad Units Variable")] [SerializeField]
    AdUnitVariable banner;

    [SerializeField] private AdUnitVariable inter;

    [SerializeField] private AdUnitVariable reward;
    public AdUnitVariable AdUnitBanner => banner;
    public AdUnitVariable AdUnitInter => inter;
    public AdUnitVariable AdUnitReward => reward;

    private float timePlay;

    private void Start()
    {
        AppControlAds.Init(this);
        ResetCounter();
    }

    public override void FixedTick()
    {
        base.FixedTick();
        if (gameStateVariable.Value == GameState.PlayingGame)
        {
            timePlay += Time.deltaTime;
        }
    }

    public void ResetCounter()
    {
        adsCounterVariable.Value = 0;
        timePlay = 0;
    }

    bool IsEnableToShowInter()
    {
        return indexLevelVariable.Value > remoteConfigLevelTurnOnInterstitial.Value &&
               adsCounterVariable.Value >= remoteConfigInterstitialCappingLevelVariable.Value &&
               timePlay >= remoteConfigInterstitialCappingTimeVariable.Value && !isOffInterAdsVariable.Value &&
               remoteConfigOnOffInterstitial.Value;
    }

    bool IsEnableToShowBanner()
    {
        return !isOffBannerVariable.Value && remoteConfigOnOffBanner.Value;
    }

    public bool IsRewardReady()
    {
        return reward.IsReady();
    }

    bool IsEnableToShowReward()
    {
        return !isOffRewardVariable.Value;
    }

    public void ShowBanner()
    {
        if (IsEnableToShowBanner())
        {
            banner.Show();
            logEventShowBanner.LogEvent();
        }
    }

    public void HideBanner()
    {
        banner.Destroy();
        logEventHideBanner.LogEvent();
    }

    public void ShowInterstitial(Action completeCallback = null, Action displayCallback = null)
    {
        if (IsEnableToShowInter())
        {
            if (inter.IsReady())
            {
                logEventRequestInter.LogEvent();
                inter.Show().OnCompleted(() =>
                {
                    completeCallback?.Invoke();
                    logEventShowInterCompleted.LogEvent();
                    ResetCounter();
                }).OnDisplayed(displayCallback);
            }
            else
            {
                completeCallback?.Invoke();
                ResetCounter();
            }
        }
        else
        {
            completeCallback?.Invoke();
            ResetCounter();
        }
    }

    public void ShowRewardAds(Action completeCallback = null, Action skipCallback = null, Action displayCallback = null,
        Action closeCallback = null)
    {
        if (IsEnableToShowReward())
        {
            if (reward.IsReady())
            {
                logEventRequestReward.LogEvent();
                reward.Show().OnCompleted(() =>
                    {
                        completeCallback?.Invoke();
                        logEventShowRewardCompleted.LogEvent();
                    }).OnDisplayed(displayCallback)
                    .OnClosed(closeCallback)
                    .OnSkipped(skipCallback);
            }
            else if (inter.IsReady())
            {
                inter.Show().OnCompleted(completeCallback).OnDisplayed(displayCallback).OnClosed(closeCallback)
                    .OnSkipped(skipCallback);
            }
            else
            {
                //completeCallback?.Invoke();
                showNotificationInGameEvent.Raise("Reward ads not ready");
            }
        }
        else
        {
            showNotificationInGameEvent.Raise("Reward ads not ready");
            //completeCallback?.Invoke();
        }
    }
}