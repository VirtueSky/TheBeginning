using System;

namespace TheBeginning.Data
{
    public partial struct UserData
    {
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
    }
}