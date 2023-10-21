using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Events;
using VirtueSky.Variables;

public class UIInGame : MonoBehaviour
{
    [FoldoutGroup(Constant.Normal_Attribute)]
    public TextMeshProUGUI LevelText;

    [FoldoutGroup(Constant.Normal_Attribute)]
    public TextMeshProUGUI LevelTypeText;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventNoParam replayEvent;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventNoParam backHomeEvent;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventNoParam nextLevelEvent;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventNoParam backLevelEvent;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private FloatEvent winLevelEvent;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private FloatEvent loseLevelEvent;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable indexLevelVariable;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private AdManagerVariable adManagerVariable;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private BooleanVariable isTestingVariable;

    private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();

    public void Start()
    {
    }

    public void OnDestroy()
    {
    }

    private void OnEnable()
    {
        Setup(indexLevelVariable.Value);
    }

    public void Setup(int currentLevel)
    {
        LevelText.text = $"Level {currentLevel}";
        LevelTypeText.text = $"Level {(Data.UseLevelABTesting == 0 ? "A" : "B")}";
    }

    public void OnClickHome()
    {
        backHomeEvent.Raise();
    }

    public void OnClickReplay()
    {
        if (isTestingVariable.Value)
        {
            replayEvent.Raise();
        }
        else
        {
            adManagerVariable.Value.ShowInterstitial(() => { replayEvent.Raise(); });
        }
    }

    public void OnClickPrevious()
    {
        backLevelEvent.Raise();
    }

    public void OnClickSkip()
    {
        if (isTestingVariable.Value)
        {
            nextLevelEvent.Raise();
        }
        else
        {
            adManagerVariable.Value.ShowRewardAds(() => { nextLevelEvent.Raise(); });
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

    public void HideUI(Level level = null)
    {
        foreach (UIEffect item in UIEffects)
        {
            item.PlayAnim();
        }
    }
}