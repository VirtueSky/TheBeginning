using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.DataStorage;
using VirtueSky.Variables;

public class GameManager : MonoBehaviour
{
    [FoldoutGroup(Constant.Normal_Attribute)]
    public LevelController levelController;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private GameStateVariable gameStateVariable;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private LoadSceneEvent loadSceneEvent;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private PopupVariable popupVariable;

    [FoldoutGroup(Constant.SO_Variable)] [SerializeField]
    private IntegerVariable indexLevelVariable;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventLevel eventWinLevel;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventLevel eventLoseLevel;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventLevel eventStartLevel;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventLevel eventSkipLevel;

    [FoldoutGroup(Constant.SO_Event)] [SerializeField]
    private EventLevel eventReplayLevel;

    [FoldoutGroup(Constant.Normal_Attribute)] [SerializeField]
    private GameObject uiInGame;

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
        gameStateVariable.Value = GameState.PrepareGame;
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
        uiInGame.SetActive(true);
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
        if (gameStateVariable.Value == GameState.WaitingResult || gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;

        gameStateVariable.Value = GameState.WinGame;
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
        if (gameStateVariable.Value == GameState.WaitingResult || gameStateVariable.Value == GameState.LoseGame ||
            gameStateVariable.Value == GameState.WinGame) return;
        gameStateVariable.Value = GameState.LoseGame;
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