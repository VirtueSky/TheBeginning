using System;
using VirtueSky.DataStorage;

namespace TheBeginning.UserData
{
    public struct UserData
    {
        #region GAME_DATA

        public static bool IsFirstOpenGame
        {
            get => Get(Constant.IS_FIRST_OPEN_GAME, 0) == 1;
            set => Set(Constant.IS_FIRST_OPEN_GAME, value ? 1 : 0);
        }

        public static int GetNumberShowGameObject(string gameObjectID)
        {
            return Get($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", 0);
        }

        public static void IncreaseNumberShowGameObject(string gameObjectID)
        {
            int value = GetNumberShowGameObject(gameObjectID);
            Set($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", ++value);
        }

        public static int ProgressAmount
        {
            get => Get(Constant.PROGRESS_AMOUNT, 0);
            set => Set(Constant.PROGRESS_AMOUNT, value);
        }

        public static bool IsItemEquipped(string itemIdentity)
        {
            return Get($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", false);
        }

        public static void SetItemEquipped(string itemIdentity, bool isEquipped = true)
        {
            Set($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", isEquipped);
        }

        public static string IdItemUnlocked = "";

        public static bool IsItemUnlocked
        {
            get => Get($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", false);
            set => Set($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", value);
        }

        public static int PercentWinGift
        {
            get => Get(Constant.PERCENT_WIN_GIFT, 0);
            set => Set(Constant.PERCENT_WIN_GIFT, value);
        }

        #endregion

        #region DAILY_REWARD

        public static bool IsClaimedTodayDailyReward()
        {
            DateTime date = DateTime.Now;
            if (!string.IsNullOrEmpty(LastDailyRewardClaimed))
            {
                try
                {
                    date = DateTime.Parse(LastDailyRewardClaimed);
                }
                catch (Exception)
                {
                    date = DateTime.Now;
                }
            }

            return (int)(DateTime.Now - date).TotalDays == 0;
        }

        public static bool IsStartLoopingDailyReward
        {
            get => Get(Constant.IS_START_LOOPING_DAILY_REWARD, 0) == 1;
            set => Set(Constant.IS_START_LOOPING_DAILY_REWARD, value ? 1 : 0);
        }

        public static string DateTimeStart
        {
            get => Get(Constant.DATE_TIME_START, DateTime.Now.ToString());
            set => Set(Constant.DATE_TIME_START, value);
        }

        public static int TotalPlayedDays =>
            (int)(DateTime.Now - DateTime.Parse(DateTimeStart)).TotalDays + 1;

        public static int DailyRewardDayIndex
        {
            get => Get(Constant.DAILY_REWARD_DAY_INDEX, 1);
            set => Set(Constant.DAILY_REWARD_DAY_INDEX, value);
        }

        public static string LastDailyRewardClaimed
        {
            get => Get(Constant.LAST_DAILY_REWARD_CLAIM, DateTime.Now.AddDays(-1).ToString());
            set => Set(Constant.LAST_DAILY_REWARD_CLAIM, value);
        }

        public static int TotalClaimDailyReward
        {
            get => Get(Constant.TOTAL_CLAIM_DAILY_REWARD, 0);
            set => Set(Constant.TOTAL_CLAIM_DAILY_REWARD, value);
        }

        #endregion

        private static T Get<T>(string key, T defaultValue = default) => GameData.Get(key, defaultValue);
        private static void Set<T>(string key, T data) => GameData.Set(key, data);
    }
}