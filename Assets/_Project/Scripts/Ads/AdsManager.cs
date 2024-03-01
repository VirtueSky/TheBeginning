using System;
using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.FirebaseTraking;
using VirtueSky.Variables;

[EditorIcon("icon_manager")]
public class AdsManager : BaseMono
{
    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private BooleanVariable isOffInterAdsVariable;
    [SerializeField] private BooleanVariable isOffBannerVariable;
    [SerializeField] private BooleanVariable isOffRewardVariable;

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

    private int adsCounter;
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

    public void AdsCounter(Level level)
    {
        adsCounter++;
    }

    public void ResetCounter()
    {
        adsCounter = 0;
        timePlay = 0;
    }

    bool IsEnableToShowInter()
    {
        return indexLevelVariable.Value > remoteConfigLevelTurnOnInterstitial.Value &&
               adsCounter >= remoteConfigInterstitialCappingLevelVariable.Value &&
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
                }).OnDisplayed(displayCallback);
            }
            else
            {
                completeCallback?.Invoke();
            }
        }
        else
        {
            completeCallback?.Invoke();
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
                completeCallback?.Invoke();
            }
        }
        else
        {
            completeCallback?.Invoke();
        }
    }
}