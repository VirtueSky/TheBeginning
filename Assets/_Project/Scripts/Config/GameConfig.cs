using UnityEngine;
using VirtueSky.Inspector;

namespace TheBeginning.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        #region Field

        [Space, HeaderLine("Level config")] [SerializeField]
        private int maxLevel = 2;

        [SerializeField] private int startLoopLevel = 1;

        [Space, HeaderLine("Gameplay config")] [SerializeField]
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

        public int MaxLevel => maxLevel;
        public int StartLoopLevel => startLoopLevel;
        public bool EnableDebugView => enableDebugView;
        public TargetFrameRate TargetFrameRate => targetFrameRate;
        public bool MultiTouchEnabled => multiTouchEnabled;
        public int WinLevelMoney => winLevelMoney;
        public int PercentWinGiftPerLevel => percentWinGiftPerLevel;
        public bool EnableNotificationInGame => enableNotificationInGame;
        public float TimeDelayHideNotificationInGame => timeDelayHideNotificationInGame;
        public bool EnableRequireInternet => enableRequireInternet;
        public float TimeDelayCheckInternet => timeDelayCheckInternet;
        public float TimeLoopCheckInternet => timeLoopCheckInternet;
        public bool EnableShowPopupUpdate => enableShowPopupUpdate;

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