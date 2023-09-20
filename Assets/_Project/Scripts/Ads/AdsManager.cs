using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Variables;

public class AdsManager : BaseMono
{
    [SerializeField] private AdManagerVariable adManagerVariable;
    [SerializeField] private GameStateVariable gameStateVariable;
    [SerializeField] IntegerVariable indexLevelVariable;
    private int adsCounter;
    private float timePlay;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        adManagerVariable.Value = this;
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
        // if purchase remove ads => return false
        if (indexLevelVariable.Value > Data.LevelTurnOnInterstitial &&
            adsCounter >= Data.CounterNumbBetweenTwoInterstitial &&
            timePlay >= Data.TimeWinBetweenTwoInterstitial && !Data.IsOffInterAds)
        {
            return true;
        }

        return false;
    }

    bool IsEnableToShowBanner()
    {
        // if purchase remove ads => return false
        return true;
    }

    public bool IsRewardReady()
    {
        return true;
    }

    public void ShowBanner()
    {
        if (IsEnableToShowBanner())
        {
            // show banner ads
        }
    }

    public void HideBanner()
    {
    }

    public void ShowInterstitial(Action completeCallback, Action displayCallback = null)
    {
        if (IsEnableToShowInter())
        {
            //show inter ads
        }
        else
        {
            completeCallback?.Invoke();
        }
    }

    public void ShowRewardAds(Action completeCallback, Action displayCallback = null,
        Action closeCallback = null, Action skipCallback = null)
    {
    }
}