using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace TheBeginning.Config
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class GameConfig : ScriptableSettings<GameConfig>
    {
        #region Field

        [HeaderLine("Gameplay config")] [SerializeField]
        private bool enableDebugView = true;

        [SerializeField] private TargetFrameRate targetFrameRate = TargetFrameRate.Frame60;
        [SerializeField] private bool multiTouchEnabled;
        [SerializeField] private int winLevelMoney = 100;
        [SerializeField] private int percentWinGiftPerLevel = 10;

        [Space, HeaderLine("Notification In Game")] [SerializeField]
        private bool enableNotificationInGame = true;

        [SerializeField] private float timeDelayHideNotificationInGame = 1.0f;

        [Space, HeaderLine("Require Internet")] [SerializeField]
        private bool enableRequireInternet = false;

        [SerializeField] private float timeDelayCheckInternet = 5;
        [SerializeField] private float timeLoopCheckInternet = .5f;

        [Space, HeaderLine("Show Popup Update")] [SerializeField]
        private bool enableShowPopupUpdate = false;

        #endregion

        #region Properties

        public static bool EnableDebugView => Instance.enableDebugView;
        public static TargetFrameRate TargetFrameRate => Instance.targetFrameRate;
        public static bool MultiTouchEnabled => Instance.multiTouchEnabled;
        public static int WinLevelMoney => Instance.winLevelMoney;
        public static int PercentWinGiftPerLevel => Instance.percentWinGiftPerLevel;
        public static bool EnableNotificationInGame => Instance.enableNotificationInGame;
        public static float TimeDelayHideNotificationInGame => Instance.timeDelayHideNotificationInGame;
        public static bool EnableRequireInternet => Instance.enableRequireInternet;
        public static float TimeDelayCheckInternet => Instance.timeDelayCheckInternet;
        public static float TimeLoopCheckInternet => Instance.timeLoopCheckInternet;
        public static bool EnableShowPopupUpdate => Instance.enableShowPopupUpdate;

        #endregion
    }


    public enum TargetFrameRate
    {
        ByDevice = -1,
        Frame60 = 60,
        Frame120 = 120,
        Frame240 = 240
    }
}