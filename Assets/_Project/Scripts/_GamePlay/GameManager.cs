using System;
using PrimeTween;
using TheBeginning.LevelSystem;
using TheBeginning.UI;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

namespace TheBeginning.Game
{
    [EditorIcon("icon_gamemanager")]
    public class GameManager : BaseMono
    {
        [HeaderLine(Constant.Normal_Attribute)] [ReadOnly] [SerializeField]
        private GameState gameState;

        [SerializeField] private Transform levelHolder;

        [HeaderLine(Constant.SO_Event)] [SerializeField]
        private EventGetGameState eventGetGameState;

        [SerializeField] private EventLoadLevel eventLoadLevel;
        [SerializeField] private EventGetCurrentLevel eventGetCurrentLevel;
        [SerializeField] private EventGetPreviousLevel eventGetPreviousLevel;
        [SerializeField] private EventNoParam callReturnHome;
        [SerializeField] private EventNoParam callReplayLevelEvent;
        [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
        [SerializeField] private EventNoParam callNextLevelEvent;
        [SerializeField] private EventNoParam callBackLevelEvent;
        [SerializeField] private FloatEvent callWinLevelEvent;
        [SerializeField] private FloatEvent callLoseLevelEvent;


        [HeaderLine(Constant.SO_Variable)] [SerializeField]
        private IntegerVariable indexLevelVariable;

        public static event Action<Level> OnStartLevelEvent;
        public static event Action<Level> OnWinLevelEvent;
        public static event Action<Level> OnLoseLevelEvent;

        public override void OnEnable()
        {
            base.OnEnable();
            eventGetGameState.AddListener(GetGameState);
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
            eventGetGameState.RemoveListener(GetGameState);
            callPlayCurrentLevelEvent.RemoveListener(PlayCurrentLevel);
            callReplayLevelEvent.RemoveListener(ReplayGame);
            callNextLevelEvent.RemoveListener(NextLevel);
            callBackLevelEvent.RemoveListener(BackLevel);
            callWinLevelEvent.RemoveListener(OnWinGame);
            callLoseLevelEvent.RemoveListener(OnLoseGame);
            callReturnHome.RemoveListener(ReturnHome);
        }

        private GameState GetGameState() => gameState;

        private void Start()
        {
            ReturnHome();
        }

        void ReturnHome()
        {
            gameState = GameState.Lobby;
            PopupManager.Show<HomePopup>();
            levelHolder.ClearTransform();
        }

        private void PlayCurrentLevel()
        {
            StartGame();
            PopupManager.Show<GameplayPopup>();
        }

        private void ReplayGame()
        {
            StartGame();
            PopupManager.Show<GameplayPopup>();
        }

        private void BackLevel()
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

        private void NextLevel()
        {
            indexLevelVariable.Value++;
            var levelPrefab = eventLoadLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(levelPrefab, levelHolder, false);
        }

        private void StartGame()
        {
            gameState = GameState.PlayingGame;
            OnStartLevelEvent?.Invoke(eventGetCurrentLevel.Raise());
            var currentLevelPrefab = eventGetCurrentLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(currentLevelPrefab, levelHolder, false);
        }

        private void OnWinGame(float delayPopupShowTime = 2.5f)
        {
            if (gameState == GameState.WaitingResult ||
                gameState == GameState.LoseGame ||
                gameState == GameState.WinGame) return;
            gameState = GameState.WinGame;
            OnWinLevelEvent?.Invoke(eventGetCurrentLevel.Raise());
            Tween.Delay(delayPopupShowTime, () =>
            {
                indexLevelVariable.Value++;
                eventLoadLevel.Raise();
                PopupManager.Show<WinPopup>();
                PopupManager.Hide<GameplayPopup>();
            });
        }

        private void OnLoseGame(float delayPopupShowTime = 2.5f)
        {
            if (gameState == GameState.WaitingResult ||
                gameState == GameState.LoseGame ||
                gameState == GameState.WinGame) return;
            gameState = GameState.LoseGame;
            OnLoseLevelEvent?.Invoke(eventGetCurrentLevel.Raise());
            Tween.Delay(delayPopupShowTime, () =>
            {
                PopupManager.Show<LosePopup>();
                PopupManager.Hide<GameplayPopup>();
            });
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
}