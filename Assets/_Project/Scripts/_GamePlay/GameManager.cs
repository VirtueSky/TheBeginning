using CodeStage.AdvancedFPSCounter;
using PrimeTween;
using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.FirebaseTraking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[EditorIcon("GameManager")]
public class GameManager : BaseMono
{
    [HeaderLine(Constant.Normal_Attribute)] [ReadOnly] [SerializeField]
    private GameState gameState;

    public LevelController levelController;

    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseStartLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseReplayLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseWinLevel;
    [SerializeField] private LogEventFirebaseOneParam logEventFirebaseLoseLevel;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventLevel eventWinLevel;

    [SerializeField] private EventLevel eventLoseLevel;
    [SerializeField] private EventLevel eventStartLevel;
    [SerializeField] private EventLevel eventSkipLevel;
    [SerializeField] private EventLevel eventReplayLevel;

    [SerializeField] private EventNoParam callReturnHome;
    [SerializeField] private EventNoParam callReplayLevelEvent;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
    [SerializeField] private EventNoParam callNextLevelEvent;
    [SerializeField] private EventNoParam callBackLevelEvent;
    [SerializeField] private EventNoParam callPrepareLevelEvent;
    [SerializeField] private FloatEvent callWinLevelEvent;
    [SerializeField] private FloatEvent callLoseLevelEvent;


    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;


    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    public override void OnEnable()
    {
        base.OnEnable();
        callPlayCurrentLevelEvent.AddListener(PlayCurrentLevel);
        callReplayLevelEvent.AddListener(ReplayGame);
        callNextLevelEvent.AddListener(NextLevel);
        callBackLevelEvent.AddListener(BackLevel);
        callPrepareLevelEvent.AddListener(PrepareLevel);
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
        callPrepareLevelEvent.RemoveListener(PrepareLevel);
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
        PrepareLevel();
    }

    public void PlayCurrentLevel()
    {
        StartGame();
        AppControlPopup.Show<PopupInGame>();
    }

    public void PrepareLevel()
    {
        GameState = GameState.PrepareGame;
        levelController.PrepareLevel();
    }

    public void ReplayGame()
    {
        eventReplayLevel.Raise(levelController.CurrentLevel);
        logEventFirebaseReplayLevel.LogEvent(levelController.CurrentLevel.name);
        PrepareLevel();
        StartGame();
        AppControlPopup.Show<PopupInGame>();
    }

    public void BackLevel()
    {
        if (indexLevelVariable?.Value > 1)
        {
            indexLevelVariable.Value--;
        }

        PrepareLevel();
        StartGame();
    }

    public void NextLevel()
    {
        eventSkipLevel.Raise(levelController.CurrentLevel);
        indexLevelVariable.Value++;
        PrepareLevel();
        StartGame();
    }

    public void StartGame()
    {
        GameState = GameState.PlayingGame;
        eventStartLevel.Raise(levelController.CurrentLevel);
        levelController.ActiveCurrentLevel(true);
        logEventFirebaseStartLevel.LogEvent(levelController.CurrentLevel.name);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (GameState == GameState.WaitingResult ||
            GameState == GameState.LoseGame ||
            GameState == GameState.WinGame) return;

        GameState = GameState.WinGame;
        eventWinLevel.Raise(levelController.CurrentLevel);
        logEventFirebaseWinLevel.LogEvent(levelController.CurrentLevel.name);
        Tween.Delay(delayPopupShowTime, () =>
        {
            indexLevelVariable.Value++;
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
        eventLoseLevel.Raise(levelController.CurrentLevel);
        logEventFirebaseLoseLevel.LogEvent(levelController.CurrentLevel.name);
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