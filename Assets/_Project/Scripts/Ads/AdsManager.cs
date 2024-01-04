using System;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.Core;
using VirtueSky.Variables;

public class AdsManager : BaseMono
{
    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private AdManagerVariable adManagerVariable;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    IntegerVariable indexLevelVariable;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private BooleanVariable isOffInterAdsVariable;

    [HeaderLine("Ad Units Variable")] [SerializeField]
    AdUnitVariable banner;

    [SerializeField] private AdUnitVariable inter;

    [SerializeField] private AdUnitVariable reward;


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
        if (inter.IsReady() && indexLevelVariable.Value > Data.LevelTurnOnInterstitial &&
            adsCounter >= Data.CounterNumbBetweenTwoInterstitial &&
            timePlay >= Data.TimeWinBetweenTwoInterstitial && !isOffInterAdsVariable.Value)
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
        return reward.IsReady();
    }

    public void ShowBanner()
    {
        if (IsEnableToShowBanner())
        {
            // show banner ads
            banner.Show();
        }
    }

    public void HideBanner()
    {
        banner.Destroy();
    }

    public void ShowInterstitial(Action completeCallback, Action displayCallback = null)
    {
        if (IsEnableToShowInter())
        {
            //show inter ads
            inter.Show().OnCompleted(completeCallback).OnDisplayed(displayCallback);
        }
        else
        {
            completeCallback?.Invoke();
        }
    }

    public void ShowRewardAds(Action completeCallback, Action displayCallback = null,
        Action closeCallback = null, Action skipCallback = null)
    {
        if (reward.IsReady())
        {
            reward.Show().OnCompleted(completeCallback).OnDisplayed(displayCallback).OnClosed(closeCallback)
                .OnSkipped(skipCallback);
        }
    }
}