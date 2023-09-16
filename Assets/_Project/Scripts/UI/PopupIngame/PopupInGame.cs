using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TMPro;
using UnityEngine;

public class PopupInGame : UIPopup
{
    [Header("Components")] public TextMeshProUGUI LevelText;
    public TextMeshProUGUI LevelTypeText;
   


    private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();

    public void Start()
    {
        Observer.WinLevel += HideUI;
        Observer.LoseLevel += HideUI;
    }

    public void OnDestroy()
    {
        Observer.WinLevel -= HideUI;
        Observer.LoseLevel -= HideUI;
    }

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        //if (!Data.IsTesting) eventShowBannerAds.Raise();
        Setup();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        //eventHideBannerAds.Raise();
    }

    public void Setup()
    {
        LevelText.text = $"Level {Data.CurrentLevel}";
        LevelTypeText.text = $"Level {(Data.UseLevelABTesting == 0 ? "A" : "B")}";
    }

    public void OnClickHome()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Observer.TrackClickButton?.Invoke(function.Name);

        GameManager.Instance.ReturnHome();
    }

    public void OnClickReplay()
    {
        if (Data.IsTesting)
        {
            GameManager.Instance.ReplayGame();
        }
        else
        {
            // eventShowInterAd.Raise(new ShowInterAdData(() =>
            // {
            //     MethodBase function = MethodBase.GetCurrentMethod();
            //     Observer.TrackClickButton?.Invoke(function.Name);
            //
            //     GameManager.Instance.ReplayGame();
            // }));
        }
    }

    public void OnClickPrevious()
    {
        GameManager.Instance.BackLevel();
    }

    public void OnClickSkip()
    {
        if (Data.IsTesting)
        {
            GameManager.Instance.NextLevel();
        }
        else
        {
            // eventShowRewardAd.Raise(new ShowRewardAdData(() =>
            // {
            //     MethodBase function = MethodBase.GetCurrentMethod();
            //     Observer.TrackClickButton?.Invoke(function.Name);
            //
            //     GameManager.Instance.NextLevel();
            // }));
        }
    }

    public void OnClickLevelA()
    {
        Data.UseLevelABTesting = 0;
        GameManager.Instance.ReplayGame();
    }

    public void OnClickLevelB()
    {
        Data.UseLevelABTesting = 1;
        GameManager.Instance.ReplayGame();
    }

    public void OnClickLose()
    {
        GameManager.Instance.OnLoseGame(1f);
    }

    public void OnClickWin()
    {
        GameManager.Instance.OnWinGame(1f);
    }

    private void HideUI(Level level = null)
    {
        foreach (UIEffect item in UIEffects)
        {
            item.PlayAnim();
        }
    }
}