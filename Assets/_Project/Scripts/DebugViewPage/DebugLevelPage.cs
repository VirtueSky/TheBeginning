using System.Collections;
using System.Threading.Tasks;
using UnityDebugSheet.Runtime.Core.Scripts;
using VirtueSky.Events;
using VirtueSky.Variables;

namespace TheBeginning.DebugViewPage
{
    public class DebugLevelPage : DefaultDebugPageBase
    {
        private GameStateVariable gameStateVariable;
        private EventNoParam callPlayCurrentLevelEvent;
        private EventNoParam callNextLevelEvent;
        private EventNoParam callPreviousLevelEvent;
        private FloatEvent callWinLevelEvent;
        private FloatEvent callLoseLevelEvent;
        private IntegerVariable indexLevel;
        private StringEvent showNotificationInGameEvent;

        protected override string Title => "Level Debug";

        public void Init(GameStateVariable _gameStateVariable, EventNoParam _callPlayCurrentLevel,
            EventNoParam _callNextLevel, EventNoParam _callPrevLevel, FloatEvent _callWinLevel,
            FloatEvent _callLoseLevel, IntegerVariable _indexLevel, StringEvent _showNotiEvent)
        {
            gameStateVariable = _gameStateVariable;
            callPlayCurrentLevelEvent = _callPlayCurrentLevel;
            callNextLevelEvent = _callNextLevel;
            callPreviousLevelEvent = _callPrevLevel;
            callWinLevelEvent = _callWinLevel;
            callLoseLevelEvent = _callLoseLevel;
            indexLevel = _indexLevel;
            showNotificationInGameEvent = _showNotiEvent;
        }

        public override Task Initialize()
        {
            AddButton("Next Level", clicked: NextLevel, icon: DebugViewStatic.IconNextDebug);
            AddButton("Prev Level", clicked: PrevLevel, icon: DebugViewStatic.IconBackDebug);
            AddButton("Win Level", clicked: WinLevel, icon: DebugViewStatic.IconWinDebug);
            AddButton("Lose Level", clicked: LoseLevel, icon: DebugViewStatic.IconLoseDebug);
            AddInputField("Input Level:", valueChanged: ChangeLevel, icon: DebugViewStatic.IconInputDebug);
            AddButton("Jump to level input", clicked: PlayCurrentLevel, icon: DebugViewStatic.IconOkeDebug);
            return base.Initialize();
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
            return gameStateVariable.Value == GameState.PlayingGame;
        }
    }
}