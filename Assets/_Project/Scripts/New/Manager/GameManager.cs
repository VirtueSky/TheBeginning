using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelController levelController;
    public GameState gameState;
    [SerializeField] private LoadSceneEvent loadSceneEvent;
    [SerializeField] private PopupVariable popupVariable;
    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        //PrepareLevel();
        Observer.StartLevel += UpdateScore;
    }

    public void PlayCurrentLevel()
    {
        PrepareLevel();
        StartGame();
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
        Observer.ReplayLevel?.Invoke(levelController.currentLevel);
        PrepareLevel();
        StartGame();
    }

    public void BackLevel()
    {
        Data.CurrentLevel--;

        PrepareLevel();
        StartGame();
    }

    public void NextLevel()
    {
        Observer.SkipLevel?.Invoke(levelController.currentLevel);
        Data.CurrentLevel++;

        PrepareLevel();
        StartGame();
    }

    public void StartGame()
    {
        gameState = GameState.PlayingGame;
        Observer.StartLevel?.Invoke(levelController.currentLevel);
        levelController.currentLevel.gameObject.SetActive(true);
    }

    public void OnWinGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame ||
            gameState == GameState.WinGame) return;
        gameState = GameState.WinGame;
        Observer.WinLevel?.Invoke(levelController.currentLevel);
        Data.CurrentLevel++;
        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() => { popupVariable?.Value.Show<PopupWin>(); });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame ||
            gameState == GameState.WinGame) return;
        gameState = GameState.LoseGame;
        Observer.LoseLevel?.Invoke(levelController.currentLevel);

        DOTween.Sequence().AppendInterval(delayPopupShowTime)
            .AppendCallback(() => { popupVariable?.Value.Show<PopupLose>(); });
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