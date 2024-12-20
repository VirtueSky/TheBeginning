using System;
using TheBeginning.Game;
using TheBeginning.LevelSystem;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Inter Variable", fileName = "inter_ad_variable")]
public class InterAdVariable : AdVariable
{
    [SerializeField] private AdUnitVariable interVariable;

    [Space, SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private BooleanVariable debugOnOffInterAdsVariable;
    [SerializeField] private IntegerVariable adsCounterVariable;
    [SerializeField] private FloatVariable timeCounterInterAds;
    [SerializeField] private EventGetGameState eventGetGameState;

    [Space, HeaderLine("Firebase Remote Config"), SerializeField]
    private IntegerVariable remoteConfigLevelTurnOnInterstitial;

    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingLevelVariable;
    [SerializeField] private IntegerVariable remoteConfigInterstitialCappingTimeVariable;
    [SerializeField] private BooleanVariable remoteConfigOnOffInterstitial;

    public AdUnitVariable AdUnitInterVariable => interVariable;

    public override void Init()
    {
        ResetCounter();
        App.SubTick(OnUpdate);
        GameManager.OnWinLevelEvent += OnEndLevel;
        GameManager.OnLoseLevelEvent += OnEndLevel;
    }

    bool Condition()
    {
        return interVariable.IsReady() && indexLevelVariable.Value > remoteConfigLevelTurnOnInterstitial.Value &&
               adsCounterVariable.Value >= remoteConfigInterstitialCappingLevelVariable.Value &&
               timeCounterInterAds.Value >= remoteConfigInterstitialCappingTimeVariable.Value &&
               debugOnOffInterAdsVariable.Value &&
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
            interVariable.Show().OnCompleted(() =>
            {
                DelayHandle(() =>
                {
                    completeCallback?.Invoke();
                    ResetCounter();
                });
            }).OnDisplayed(displayCallback);
        }
        else
        {
            completeCallback?.Invoke();
        }
    }


    void DelayHandle(Action action)
    {
        App.Delay(0.05f, action);
    }

    void OnEndLevel(Level level)
    {
        adsCounterVariable.Value++;
    }

    void OnUpdate()
    {
        if (eventGetGameState.Raise() == GameState.PlayingGame) timeCounterInterAds.Value += Time.deltaTime;
    }
}