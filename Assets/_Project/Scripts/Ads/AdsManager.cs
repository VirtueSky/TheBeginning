using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Variables;

public class AdsManager : BaseMono
{
    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private AdManagerVariable adManagerVariable;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    IntegerVariable indexLevelVariable;
    
    [Header("Ad Units Variable")]
    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    AdUnitVariable banner;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private AdUnitVariable inter;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private AdUnitVariable reward;

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
            reward.Show().OnCompleted(completeCallback).OnDisplayed(displayCallback).OnClosed(closeCallback).OnSkipped(skipCallback);
        }
    }
}