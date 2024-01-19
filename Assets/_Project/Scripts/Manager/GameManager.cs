using CodeStage.AdvancedFPSCounter;
using PrimeTween;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class GameManager : MonoBehaviour
{
    [HeaderLine(Constant.Normal_Attribute)]
    public LevelController levelController;

    [HeaderLine(Constant.SO_Event)] [SerializeField]
    private EventLevel eventWinLevel;

    [SerializeField] private EventLevel eventLoseLevel;

    [SerializeField] private EventLevel eventStartLevel;

    [SerializeField] private EventLevel eventSkipLevel;

    [SerializeField] private EventLevel eventReplayLevel;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [SerializeField] private PopupVariable popupVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;


    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    void Awake()
    {
        // Application.targetFrameRate = 60;
    }

    void Start()
    {
        PlayCurrentLevel();
    }

    public void PlayCurrentLevel()
    {
        PrepareLevel();
        StartGame();
        popupVariable.Value.Show<PopupInGame>();
    }

    public void UpdateScore(Level level)
    {
        //  if (AuthService.Instance.isLoggedIn && AuthService.Instance.IsCompleteSetupName)
        //   {
        //      AuthService.UpdatePlayerStatistics("RANK_LEVEL", Data.CurrentLevel);
        //  }
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
        PrepareLevel();
        StartGame();
        popupVariable.Value.Show<PopupInGame>();
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
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameStateVariable.Value == GameState.WaitingResult ||
            gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;

        gameStateVariable.Value = GameState.WinGame;
        eventWinLevel.Raise(levelController.currentLevel);
        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() =>
            {
                indexLevelVariable.Value++;
                popupVariable?.Value.Show<PopupWin>();
                popupVariable.Value.Hide<PopupInGame>();
            });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameStateVariable.Value == GameState.WaitingResult ||
            gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;
        gameStateVariable.Value = GameState.LoseGame;
        eventLoseLevel.Raise(levelController.currentLevel);
        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() =>
            {
                popupVariable?.Value.Show<PopupLose>();
                popupVariable.Value.Hide<PopupInGame>();
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