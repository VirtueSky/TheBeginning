using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.Events;
using VirtueSky.Variables;

public class UIInGame : MonoBehaviour
{
    [HeaderLine(Constant.Normal_Attribute)]
    public TextMeshProUGUI LevelText;


    public TextMeshProUGUI LevelTypeText;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventNoParam replayEvent;

    [SerializeField] private EventNoParam backHomeEvent;

    [SerializeField] private EventNoParam nextLevelEvent;

    [SerializeField] private EventNoParam backLevelEvent;

    [SerializeField] private FloatEvent winLevelEvent;

    [SerializeField] private FloatEvent loseLevelEvent;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable indexLevelVariable;

    [SerializeField] private AdManagerVariable adManagerVariable;

    [SerializeField] private BooleanVariable isTestingVariable;

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