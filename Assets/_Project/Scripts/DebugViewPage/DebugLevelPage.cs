using System.Collections;
using System.Threading.Tasks;
using TheBeginning.Game;
using UnityDebugSheet.Runtime.Core.Scripts;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugLevelPage : DefaultDebugPageBase
    {
        private EventGetGameState eventGetGameState;
        private EventNoParam callPlayCurrentLevelEvent;
        private EventNoParam callNextLevelEvent;
        private EventNoParam callPreviousLevelEvent;
        private FloatEvent callWinLevelEvent;
        private FloatEvent callLoseLevelEvent;
        private IntegerVariable indexLevel;
        private StringEvent showNotificationInGameEvent;
        private Sprite iconNext;
        private Sprite iconBack;
        private Sprite iconWin;
        private Sprite iconLose;
        private Sprite iconInput;
        private Sprite iconOk;

        protected override string Title => "Level Debug";

        public void Init(EventGetGameState _eventGetGameState, EventNoParam _callPlayCurrentLevel,
            EventNoParam _callNextLevel, EventNoParam _callPrevLevel, FloatEvent _callWinLevel,
            FloatEvent _callLoseLevel, IntegerVariable _indexLevel, StringEvent _showNotiEvent, Sprite _iconNext,
            Sprite _iconBack, Sprite _iconWin, Sprite _iconLose, Sprite _iconInput, Sprite _iconOk)
        {
            eventGetGameState = _eventGetGameState;
            callPlayCurrentLevelEvent = _callPlayCurrentLevel;
            callNextLevelEvent = _callNextLevel;
            callPreviousLevelEvent = _callPrevLevel;
            callWinLevelEvent = _callWinLevel;
            callLoseLevelEvent = _callLoseLevel;
            indexLevel = _indexLevel;
            showNotificationInGameEvent = _showNotiEvent;
            iconNext = _iconNext;
            iconBack = _iconBack;
            iconWin = _iconWin;
            iconLose = _iconLose;
            iconInput = _iconInput;
            iconOk = _iconOk;
        }


#if UDS_USE_ASYNC_METHODS
        public override Task Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#else
        public override IEnumerator Initialize()
        {
            OnInitialize();
            return base.Initialize();
        }
#endif

        void OnInitialize()
        {
            AddButton("Next Level", clicked: NextLevel, icon: iconNext);
            AddButton("Prev Level", clicked: PrevLevel, icon: iconBack);
            AddButton("Win Level", clicked: WinLevel, icon: iconWin);
            AddButton("Lose Level", clicked: LoseLevel, icon: iconLose);
            AddInputField("Input Level:", valueChanged: ChangeLevel, icon: iconInput);
            AddButton("Jump to level input", clicked: PlayCurrentLevel, icon: iconOk);
        }

        void ChangeLevel(string s)
        {
            if (IsPlayingGame())
            {
                indexLevel.Value = int.Parse(s);
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        void PlayCurrentLevel()
        {
            if (IsPlayingGame())
            {
                callPlayCurrentLevelEvent.Raise();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        void NextLevel()
        {
            if (IsPlayingGame())
            {
                callNextLevelEvent.Raise();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        void PrevLevel()
        {
            if (IsPlayingGame())
            {
                callPreviousLevelEvent.Raise();
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        void WinLevel()
        {
            if (IsPlayingGame())
            {
                callWinLevelEvent.Raise(1);
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        void LoseLevel()
        {
            if (IsPlayingGame())
            {
                callLoseLevelEvent.Raise(1);
            }
            else
            {
                showNotificationInGameEvent.Raise("Only works when you play games");
            }
        }

        private bool IsPlayingGame()
        {
            return eventGetGameState.Raise() == GameState.PlayingGame;
        }
    }
}