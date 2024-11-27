using System;
using PrimeTween;
using TheBeginning.LevelSystem;
using TheBeginning.UI;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;
using VirtueSky.Variables;

namespace TheBeginning.Game
{
    [EditorIcon("icon_gamemanager")]
    public class GameManager : BaseMono
    {
        [HeaderLine(Constant.Normal_Attribute)] [ReadOnly] [SerializeField]
        private GameState gameState;

        [SerializeField] private TrackingFirebaseOneParam trackingFirebaseStartLevel;
        [SerializeField] private TrackingFirebaseOneParam trackingFirebaseReplayLevel;
        [SerializeField] private TrackingFirebaseOneParam trackingFirebaseWinLevel;
        [SerializeField] private TrackingFirebaseOneParam trackingFirebaseLoseLevel;
        [SerializeField] private Transform levelHolder;

        [HeaderLine(Constant.SO_Event)] [SerializeField]
        private EventGetGameState eventGetGameState;

        [SerializeField] private EventLoadLevel eventLoadLevel;
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
        private IntegerVariable indexLevelVariable;

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
            PopupManager.Show<PopupHome>();
            levelHolder.ClearTransform();
        }

        private void PlayCurrentLevel()
        {
            StartGame();
            PopupManager.Show<PopupInGame>();
        }

        private void ReplayGame()
        {
            eventReplayLevel.Raise(eventGetCurrentLevel.Raise());
            trackingFirebaseReplayLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
            StartGame();
            PopupManager.Show<PopupInGame>();
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
            eventSkipLevel.Raise(eventGetCurrentLevel.Raise());
            indexLevelVariable.Value++;
            var levelPrefab = eventLoadLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(levelPrefab, levelHolder, false);
        }

        private void StartGame()
        {
            gameState = GameState.PlayingGame;
            eventStartLevel.Raise(eventGetCurrentLevel.Raise());
            var currentLevelPrefab = eventGetCurrentLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(currentLevelPrefab, levelHolder, false);
            trackingFirebaseStartLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
        }

        private void OnWinGame(float delayPopupShowTime = 2.5f)
        {
            if (gameState == GameState.WaitingResult ||
                gameState == GameState.LoseGame ||
                gameState == GameState.WinGame) return;
            gameState = GameState.WinGame;
            eventWinLevel.Raise(eventGetCurrentLevel.Raise());
            trackingFirebaseWinLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
            Tween.Delay(delayPopupShowTime, () =>
            {
                indexLevelVariable.Value++;
                eventLoadLevel.Raise();
                PopupManager.Show<PopupWin>();
                PopupManager.Hide<PopupInGame>();
            });
        }

        private void OnLoseGame(float delayPopupShowTime = 2.5f)
        {
            if (gameState == GameState.WaitingResult ||
                gameState == GameState.LoseGame ||
                gameState == GameState.WinGame) return;
            gameState = GameState.LoseGame;
            eventLoseLevel.Raise(eventGetCurrentLevel.Raise());
            trackingFirebaseLoseLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
            Tween.Delay(delayPopupShowTime, () =>
            {
                PopupManager.Show<PopupLose>();
                PopupManager.Hide<PopupInGame>();
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