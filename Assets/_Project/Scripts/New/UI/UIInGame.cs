using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using VirtueSky.Events;

public class UIInGame : MonoBehaviour
{
    [Header("Components")] public TextMeshProUGUI LevelText;
    public TextMeshProUGUI LevelTypeText;
    [SerializeField] private EventNoParam replayEvent;
    [SerializeField] private EventNoParam backHomeEvent;
    [SerializeField] private EventNoParam nextLevelEvent;
    [SerializeField] private EventNoParam backLevelEvent;
    [SerializeField] private FloatEvent winLevelEvent;
    [SerializeField] private FloatEvent loseLevelEvent;


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

    private void OnEnable()
    {
        Setup();
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
        backHomeEvent.Raise();
    }

    public void OnClickReplay()
    {
        if (Data.IsTesting)
        {
            replayEvent.Raise();
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
        backLevelEvent.Raise();
    }

    public void OnClickSkip()
    {
        if (Data.IsTesting)
        {
            nextLevelEvent.Raise();
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
        replayEvent.Raise();
    }

    public void OnClickLevelB()
    {
        Data.UseLevelABTesting = 1;
        replayEvent.Raise();
    }

    public void OnClickLose()
    {
        loseLevelEvent.Raise(1);
    }

    public void OnClickWin()
    {
        winLevelEvent.Raise(1);
    }

    private void HideUI(Level level = null)
    {
        foreach (UIEffect item in UIEffects)
        {
            item.PlayAnim();
        }
    }
}