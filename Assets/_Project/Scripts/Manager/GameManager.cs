using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.DataStorage;
using VirtueSky.Variables;

public class GameManager : MonoBehaviour
{
    public LevelController levelController;
    public GameState gameState;
    [SerializeField] private LoadSceneEvent loadSceneEvent;
    [SerializeField] private PopupVariable popupVariable;
    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private EventLevel eventWinLevel;
    [SerializeField] private EventLevel eventLoseLevel;
    [SerializeField] private EventLevel eventStartLevel;
    [SerializeField] private EventLevel eventSkipLevel;
    [SerializeField] private EventLevel eventReplayLevel;
    [SerializeField] private GameObject uiInGame;
    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        PlayCurrentLevel();
    }

    public void PlayCurrentLevel()
    {
        PrepareLevel();
        StartGame();
        uiInGame.SetActive(true);
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
        gameState = GameState.PrepareGame;
        levelController.PrepareLevel();
    }

    public void ReturnHome()
    {
        loadSceneEvent.Raise(new LoadSceneData(false, Constant.HOME_SCENE, .1f, null));
    }

    public void ReplayGame()
    {
        eventReplayLevel.Raise(levelController.currentLevel);
        PrepareLevel();
        StartGame();
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
        gameState = GameState.PlayingGame;
        eventStartLevel.Raise(levelController.currentLevel);
        levelController.currentLevel.gameObject.SetActive(true);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame ||
            gameState == GameState.WinGame) return;
        gameState = GameState.WinGame;
        eventWinLevel.Raise(levelController.currentLevel);
        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() =>
            {
                indexLevelVariable.Value++;
                popupVariable?.Value.Show<PopupWin>();
                uiInGame.SetActive(false);
            });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame ||
            gameState == GameState.WinGame) return;
        gameState = GameState.LoseGame;
        eventLoseLevel.Raise(levelController.currentLevel);
        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() =>
            {
                popupVariable?.Value.Show<PopupLose>();
                uiInGame.SetActive(false);
            });
    }

    public void ChangeAFpsState()
    {
        if (Data.IsTesting)
        {
            AFpsCounter.enabled = !AFpsCounter.isActiveAndEnabled;
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
}