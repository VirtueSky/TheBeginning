using CodeStage.AdvancedFPSCounter;
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
        [SerializeField] private FloatVariable timeCounterInterAdVariable;

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

        private async void NextLevel()
        {
            eventSkipLevel.Raise(eventGetCurrentLevel.Raise());
            indexLevelVariable.Value++;
            var levelPrefab = await eventLoadLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(levelPrefab, levelHolder, false);
        }

        private void StartGame()
        {
            GameState = GameState.PlayingGame;
            eventStartLevel.Raise(eventGetCurrentLevel.Raise());
            var currentLevelPrefab = eventGetCurrentLevel.Raise();
            levelHolder.ClearTransform();
            Instantiate(currentLevelPrefab, levelHolder, false);
            trackingFirebaseStartLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
        }

        private void OnWinGame(float delayPopupShowTime = 2.5f)
        {
            if (GameState == GameState.WaitingResult ||
                GameState == GameState.LoseGame ||
                GameState == GameState.WinGame) return;
            GameState = GameState.WinGame;
            eventWinLevel.Raise(eventGetCurrentLevel.Raise());
            adsCounterVariable.Value++;
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
            if (GameState == GameState.WaitingResult ||
                GameState == GameState.LoseGame ||
                GameState == GameState.WinGame) return;
            GameState = GameState.LoseGame;
            eventLoseLevel.Raise(eventGetCurrentLevel.Raise());
            adsCounterVariable.Value++;
            trackingFirebaseLoseLevel.TrackEvent(eventGetCurrentLevel.Raise().name);
            Tween.Delay(delayPopupShowTime, () =>
            {
                PopupManager.Show<PopupLose>();
                PopupManager.Hide<PopupInGame>();
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

        public override void FixedTick()
        {
            base.FixedTick();
            if (GameState == GameState.PlayingGame)
            {
                timeCounterInterAdVariable.Value += Time.deltaTime;
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
}