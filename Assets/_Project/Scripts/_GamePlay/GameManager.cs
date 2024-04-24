using CodeStage.AdvancedFPSCounter;
using PrimeTween;
using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.FirebaseTracking;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

[EditorIcon("icon_gamemanager")]
public class GameManager : BaseMono
{
    [HeaderLine(Constant.Normal_Attribute)] [ReadOnly] [SerializeField]
    private GameState gameState;

    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseStartLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseReplayLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseWinLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseLoseLevel;
    [SerializeField] private Transform levelHolder;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventLoadLevel eventLoadLevel;

    [SerializeField] private EventGetCurrentLevel eventGetCurrentLevel;
    [SerializeField] private EventGetPreviousLevel eventGetPreviousLevel;
    [SerializeField] private EventLevel eventWinLevel;
    [SerializeField] private EventLevel eventLoseLevel;
    [SerializeField] private EventLevel eventStartLevel;
    [SerializeField] private EventLevel eventSkipLevel;
    [SerializeField] private EventLevel eventReplayLevel;
    [SerializeField] private EventNoParam callReturnHome;
    [SerializeField] private EventNoParam callReplayLevelEvent;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
    [SerializeField] private EventNoParam callNextLevelEvent;
    [SerializeField] private EventNoParam callBackLevelEvent;
    [SerializeField] private FloatEvent callWinLevelEvent;
    [SerializeField] private FloatEvent callLoseLevelEvent;


    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private IntegerVariable adsCounterVariable;


    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    public override void OnEnable()
    {
        base.OnEnable();
        callPlayCurrentLevelEvent.AddListener(PlayCurrentLevel);
        callReplayLevelEvent.AddListener(ReplayGame);
        callNextLevelEvent.AddListener(NextLevel);
        callBackLevelEvent.AddListener(BackLevel);
        callWinLevelEvent.AddListener(OnWinGame);
        callLoseLevelEvent.AddListener(OnLoseGame);
        callReturnHome.AddListener(ReturnHome);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        callPlayCurrentLevelEvent.RemoveListener(PlayCurrentLevel);
        callReplayLevelEvent.RemoveListener(ReplayGame);
        callNextLevelEvent.RemoveListener(NextLevel);
        callBackLevelEvent.RemoveListener(BackLevel);
        callWinLevelEvent.RemoveListener(OnWinGame);
        callLoseLevelEvent.RemoveListener(OnLoseGame);
        callReturnHome.RemoveListener(ReturnHome);
    }

    void Start()
    {
        ReturnHome();
    }

    void ReturnHome()
    {
        GameState = GameState.Lobby;
        AppControlPopup.Show<PopupHome>();
        levelHolder.ClearTransform();
    }

    public void PlayCurrentLevel()
    {
        StartGame();
        AppControlPopup.Show<PopupInGame>();
    }

    public void ReplayGame()
    {
        eventReplayLevel.Raise(eventGetCurrentLevel.Raise());
        logEventFirebaseReplayLevel.LogEvent(eventGetCurrentLevel.Raise().name);
        StartGame();
        AppControlPopup.Show<PopupInGame>();
    }

    public void BackLevel()
    {
        if (indexLevelVariable?.Value > 1)
        {
            indexLevelVariable.Value--;
        }

        var levelPrefab = eventGetPreviousLevel.Raise();
        levelHolder.ClearTransform();
        Instantiate(levelPrefab, levelHolder, false);
        eventLoadLevel.Raise();
    }

    public async void NextLevel()
    {
        eventSkipLevel.Raise(eventGetCurrentLevel.Raise());
        indexLevelVariable.Value++;
        var levelPrefab = await eventLoadLevel.Raise();
        levelHolder.ClearTransform();
        Instantiate(levelPrefab, levelHolder, false);
    }

    public void StartGame()
    {
        GameState = GameState.PlayingGame;
        eventStartLevel.Raise(eventGetCurrentLevel.Raise());
        var currentLevelPrefab = eventGetCurrentLevel.Raise();
        levelHolder.ClearTransform();
        Instantiate(currentLevelPrefab, levelHolder, false);
        logEventFirebaseStartLevel.LogEvent(eventGetCurrentLevel.Raise().name);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (GameState == GameState.WaitingResult ||
            GameState == GameState.LoseGame ||
            GameState == GameState.WinGame) return;
        GameState = GameState.WinGame;
        eventWinLevel.Raise(eventGetCurrentLevel.Raise());
        adsCounterVariable.Value++;
        logEventFirebaseWinLevel.LogEvent(eventGetCurrentLevel.Raise().name);
        Tween.Delay(delayPopupShowTime, () =>
        {
            indexLevelVariable.Value++;
            eventLoadLevel.Raise();
            AppControlPopup.Show<PopupWin>();
            AppControlPopup.Hide<PopupInGame>();
        });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (GameState == GameState.WaitingResult ||
            GameState == GameState.LoseGame ||
            GameState == GameState.WinGame) return;
        GameState = GameState.LoseGame;
        eventLoseLevel.Raise(eventGetCurrentLevel.Raise());
        adsCounterVariable.Value++;
        logEventFirebaseLoseLevel.LogEvent(eventGetCurrentLevel.Raise().name);
        Tween.Delay(delayPopupShowTime, () =>
        {
            AppControlPopup.Show<PopupLose>();
            AppControlPopup.Hide<PopupInGame>();
        });
    }

    public void ChangeAFpsState()
    {
        AFpsCounter.enabled = !AFpsCounter.isActiveAndEnabled;
    }

    private GameState GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            gameStateVariable.Value = gameState;
        }
    }
}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    WaitingResult,
    LoseGame,
    WinGame,
    Lobby
}