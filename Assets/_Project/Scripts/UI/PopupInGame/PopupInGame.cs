using System.Collections.Generic;
using System.Linq;
using TheBeginning.AppControl;
using TMPro;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class PopupInGame : UIPopup
{
    [HeaderLine(Constant.Normal_Attribute)]
    public TextMeshProUGUI LevelText;


    public TextMeshProUGUI LevelTypeText;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventNoParam replayEvent;

    [SerializeField] private StringEvent changeSceneEvent;

    [SerializeField] private EventNoParam nextLevelEvent;

    [SerializeField] private EventNoParam backLevelEvent;

    [SerializeField] private FloatEvent winLevelEvent;

    [SerializeField] private FloatEvent loseLevelEvent;
    [SerializeField] private StringEvent playAnimCharacterEvent;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable indexLevelVariable;

    private List<UIEffect> UIEffects => GetComponentsInChildren<UIEffect>().ToList();


    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        Setup(indexLevelVariable.Value);
        indexLevelVariable.AddListener(Setup);
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        indexLevelVariable.RemoveListener(Setup);
    }

    public void Setup(int currentLevel)
    {
        LevelText.text = $"Level {currentLevel}";
        // LevelTypeText.text = $"Level ";
    }

    public void OnClickHome()
    {
        changeSceneEvent.Raise(Constant.HOME_SCENE);
    }

    public void OnClickReplay()
    {
        AppControlAds.ShowInterstitial(() => { replayEvent.Raise(); });
    }

    public void OnClickPrevious()
    {
        backLevelEvent.Raise();
    }

    public void OnClickSkip()
    {
        AppControlAds.ShowReward(() => { nextLevelEvent.Raise(); });
    }

    public void OnClickLose()
    {
        loseLevelEvent.Raise(1);
    }

    public void OnClickWin()
    {
        winLevelEvent.Raise(1);
    }

    public void OnClickPlayAnim(string animationName)
    {
        playAnimCharacterEvent.Raise(animationName);
    }

    public void HideUI(Level level = null)
    {
        if (UIEffects.Count == 0) return;
        foreach (UIEffect item in UIEffects)
        {
            item.PlayAnim();
        }
    }
}