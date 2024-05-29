using System;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.FirebaseTracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Inter Variable", fileName = "inter_ad_variable")]
public class InterAdVariable : BaseSO
{
    [SerializeField] private AdUnitVariable interVariable;

    [Space, SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private BooleanVariable isOffInterAdsVariable;
    [SerializeField] private IntegerVariable adsCounterVariable;
    [SerializeField] private FloatVariable timeCounterInterAds;

    [Space, HeaderLine("Firebase Remote Config"), SerializeField]
    private IntegerVariable remoteConfigLevelTurnOnInterstitial;

    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingLevelVariable;
    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingTimeVariable;
    [SerializeField] private BooleanVariable remoteConfigOnOffInterstitial;

    [Space, HeaderLine("Log Event Firebase Analytic"), SerializeField]
    private LogEventFirebaseNoParam logEventRequestInter;

    [SerializeField] private LogEventFirebaseNoParam logEventShowInterCompleted;
    public AdUnitVariable AdUnitInterVariable => interVariable;

    bool Condition()
    {
        return interVariable.IsReady() && indexLevelVariable.Value > remoteConfigLevelTurnOnInterstitial.Value &&
               adsCounterVariable.Value >= remoteConfigInterstitialCappingLevelVariable.Value &&
               timeCounterInterAds.Value >= remoteConfigInterstitialCappingTimeVariable.Value &&
               !isOffInterAdsVariable.Value &&
               remoteConfigOnOffInterstitial.Value;
    }

    void ResetCounter()
    {
        adsCounterVariable.Value = 0;
        timeCounterInterAds.Value = 0;
    }

    public void Show(Action completeCallback = null, Action displayCallback = null)
    {
        if (Condition())
        {
            logEventRequestInter.LogEvent();
            interVariable.Show().OnCompleted(() =>
            {
                DelayHandle(() =>
                {
                    completeCallback?.Invoke();
                    logEventShowInterCompleted.LogEvent();
                    ResetCounter();
                });
            }).OnDisplayed(displayCallback);
        }
        else
        {
            completeCallback?.Invoke();
            ResetCounter();
        }
    }


    void DelayHandle(Action action)
    {
        App.Delay(0.05f, action);
    }
}