using CodeStage.AdvancedFPSCounter;
using PrimeTween;
using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.FirebaseTraking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class GameManager : MonoBehaviour
{
    [HeaderLine(Constant.Normal_Attribute)]
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


    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;


    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    void Start()
    {
        PlayCurrentLevel();
    }

    public void PlayCurrentLevel()
    {
        PrepareLevel();
        StartGame();
        AppControlPopup.Show<PopupInGame>();
    }

    public void PrepareLevel()
    {
        gameStateVariable.Value = GameState.PrepareGame;
        levelController.PrepareLevel();
    }

    public void ReturnHome()
    {
    }

    public void ReplayGame()
    {
        eventReplayLevel.Raise(levelController.currentLevel);
        logEventFirebaseReplayLevel.LogEvent(levelController.currentLevel.name);
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
        eventSkipLevel.Raise(levelController.currentLevel);
        indexLevelVariable.Value++;
        PrepareLevel();
        StartGame();
    }

    public void StartGame()
    {
        gameStateVariable.Value = GameState.PlayingGame;
        eventStartLevel.Raise(levelController.currentLevel);
        levelController.currentLevel.gameObject.SetActive(true);
        logEventFirebaseStartLevel.LogEvent(levelController.currentLevel.name);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameStateVariable.Value == GameState.WaitingResult ||
            gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;

        gameStateVariable.Value = GameState.WinGame;
        eventWinLevel.Raise(levelController.currentLevel);
        logEventFirebaseWinLevel.LogEvent(levelController.currentLevel.name);
        Tween.Delay(delayPopupShowTime, () =>
        {
            indexLevelVariable.Value++;
            AppControlPopup.Show<PopupWin>();
            AppControlPopup.Hide<PopupInGame>();
        });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameStateVariable.Value == GameState.WaitingResult ||
            gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;
        gameStateVariable.Value = GameState.LoseGame;
        eventLoseLevel.Raise(levelController.currentLevel);
        logEventFirebaseLoseLevel.LogEvent(levelController.currentLevel.name);
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
}

public enum GameState
{
    PrepareGame,
    PlayingGame,
    WaitingResult,
    LoseGame,
    WinGame,
}