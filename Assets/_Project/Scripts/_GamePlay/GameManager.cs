using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public LevelController levelController;
    public GameState gameState;
    public AFPSCounter AFpsCounter => GetComponent<AFPSCounter>();

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        ReturnHome();

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
        PrepareLevel();
        // PopupController.Instance.HideAll();
        // PopupController.Instance.Show<PopupBackground>();
        // PopupController.Instance.Show<PopupHome>();
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
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.WinGame;
        Observer.WinLevel?.Invoke(levelController.currentLevel);
        Data.CurrentLevel++;
        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            // PopupController.Instance.HideAll();
            // PopupWin popupWin = PopupController.Instance.Get<PopupWin>() as PopupWin;
            // popupWin.SetupMoneyWin(levelController.currentLevel.BonusMoney);
            // popupWin.Show();
        });
    }

    public void OnLoseGame(float delayPopupShowTime = 2.5f)
    {
        if (gameState == GameState.WaitingResult || gameState == GameState.LoseGame || gameState == GameState.WinGame) return;
        gameState = GameState.LoseGame;
        Observer.LoseLevel?.Invoke(levelController.currentLevel);

        DOTween.Sequence().AppendInterval(delayPopupShowTime).AppendCallback(() =>
        {
            // PopupController.Instance.Hide<PopupInGame>();
            // PopupController.Instance.Show<PopupLose>();
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