using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public static partial class Data
{
    #region GAME_DATA

    public static bool IsFirstOpenGame
    {
        get => PlayerPrefs.GetInt(Constant.IS_FIRST_OPEN_GAME, 0) == 1;
        set => PlayerPrefs.SetInt(Constant.IS_FIRST_OPEN_GAME, value ? 1 : 0);
    }

    public static bool IsTesting
    {
        get => PlayerPrefs.GetInt(Constant.IS_TESTING, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(Constant.IS_TESTING, value ? 1 : 0);
            Observer.DebugChanged?.Invoke();
        }
    }

    public static bool IsOffInterAds
    {
        get => GetBool(Constant.IS_OFF_INTER_ADS, false);
        set => SetBool(Constant.IS_OFF_INTER_ADS, value);
    }

    // public static int CurrentLevel
    // {
    //     get { return GetInt(Constant.INDEX_LEVEL_CURRENT, 1); }
    //
    //     set
    //     {
    //         SetInt(Constant.INDEX_LEVEL_CURRENT, value >= 1 ? value : 1);
    //         Observer.CurrentLevelChanged?.Invoke();
    //     }
    // }

    public static int GetNumberShowGameObject(string gameObjectID)
    {
        return GetInt($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", 0);
    }

    public static void IncreaseNumberShowGameObject(string gameObjectID)
    {
        int value = GetNumberShowGameObject(gameObjectID);
        SetInt($"{Constant.GAMEOBJECT_SHOW}_{gameObjectID}", ++value);
    }

    public static int CurrencyTotal
    {
        get => GetInt(Constant.CURRENCY_TOTAL, 0);
        set
        {
            Observer.SaveCurrencyTotal?.Invoke();
            SetInt(Constant.CURRENCY_TOTAL, value);
            Observer.CurrencyTotalChanged?.Invoke();
        }
    }

    public static int ProgressAmount
    {
        get => GetInt(Constant.PROGRESS_AMOUNT, 0);
        set => SetInt(Constant.PROGRESS_AMOUNT, value);
    }

    public static bool IsItemEquipped(string itemIdentity)
    {
        return GetBool($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}");
    }

    public static void SetItemEquipped(string itemIdentity, bool isEquipped = true)
    {
        SetBool($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", isEquipped);
    }

    public static string IdItemUnlocked = "";

    public static bool IsItemUnlocked
    {
        get => GetBool($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}");
        set => SetBool($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", value);
    }

    #endregion

    #region SETTING_DATA

    public static bool BgSoundState
    {
        get => GetBool(Constant.BACKGROUND_SOUND_STATE, true);
        set
        {
            SetBool(Constant.BACKGROUND_SOUND_STATE, value);
            Observer.MusicChanged?.Invoke();
        }
    }

    public static bool FxSoundState
    {
        get => GetBool(Constant.FX_SOUND_STATE, true);
        set
        {
            SetBool(Constant.FX_SOUND_STATE, value);
            Observer.SoundChanged?.Invoke();
        }
    }

    public static bool VibrateState
    {
        get => GetBool(Constant.VIBRATE_STATE, true);
        set => SetBool(Constant.VIBRATE_STATE, value);
    }

    #endregion

    #region DAILY_REWARD

    public static bool IsClaimedTodayDailyReward()
    {
        return (int)(DateTime.Now - DateTime.Parse(LastDailyRewardClaimed)).TotalDays == 0;
    }

    public static bool IsStartLoopingDailyReward
    {
        get => PlayerPrefs.GetInt(Constant.IS_START_LOOPING_DAILY_REWARD, 0) == 1;
        set => PlayerPrefs.SetInt(Constant.IS_START_LOOPING_DAILY_REWARD, value ? 1 : 0);
    }

    public static string DateTimeStart
    {
        get => GetString(Constant.DATE_TIME_START, DateTime.Now.ToString());
        set => SetString(Constant.DATE_TIME_START, value);
    }

    public static int TotalPlayedDays =>
        (int)(DateTime.Now - DateTime.Parse(DateTimeStart)).TotalDays + 1;

    public static int DailyRewardDayIndex
    {
        get => GetInt(Constant.DAILY_REWARD_DAY_INDEX, 1);
        set => SetInt(Constant.DAILY_REWARD_DAY_INDEX, value);
    }

    public static string LastDailyRewardClaimed
    {
        get => GetString(Constant.LAST_DAILY_REWARD_CLAIM, DateTime.Now.AddDays(-1).ToString());
        set => SetString(Constant.LAST_DAILY_REWARD_CLAIM, value);
    }

    public static int TotalClaimDailyReward
    {
        get => GetInt(Constant.TOTAL_CLAIM_DAILY_REWARD, 0);
        set => SetInt(Constant.TOTAL_CLAIM_DAILY_REWARD, value);
    }

    #endregion

    #region PLAYFAB_DATA

    public static string PlayfabLoginId
    {
        get => GetString(Constant.PLAYFAB_LOGIN_ID, null);
        set => SetString(Constant.PLAYFAB_LOGIN_ID, value);
    }

    public static string PlayerName
    {
        get => GetString(Constant.PLAYER_NAME, null);
        set => SetString(Constant.PLAYER_NAME, value);
    }

    public static string PlayerId
    {
        get => GetString(Constant.PLAYER_ID, null);
        set => SetString(Constant.PLAYER_ID, value);
    }

    public static string PlayerCountryCode
    {
        get => GetString(Constant.PLAYER_COUNTRY_CODE, null);
        set => SetString(Constant.PLAYER_COUNTRY_CODE, value);
    }

    public static PlayerProfileModel PlayerProfile;

    public static int PercentWinGift
    {
        get => GetInt(Constant.PERCENT_WIN_GIFT, 0);
        set => SetInt(Constant.PERCENT_WIN_GIFT, value);
    }

    #endregion

    #region FIREBASE

    // TOGGLE LEVEL AB TESTING? 0:NO, 1:YES
    public static int DEFAULT_USE_LEVEL_AB_TESTING = 0;

    public static int UseLevelABTesting
    {
        get => PlayerPrefs.GetInt(Constant.USE_LEVEL_AB_TESTING, DEFAULT_USE_LEVEL_AB_TESTING);
        set => PlayerPrefs.SetInt(Constant.USE_LEVEL_AB_TESTING, value);
    }

    // SET LEVEL TO ENABLE INTERSTITIAL
    public static int DEFAULT_LEVEL_TURN_ON_INTERSTITIAL = 5;

    public static int LevelTurnOnInterstitial
    {
        get => PlayerPrefs.GetInt(Constant.LEVEL_TURN_ON_INTERSTITIAL,
            DEFAULT_LEVEL_TURN_ON_INTERSTITIAL);
        set => PlayerPrefs.SetInt(Constant.LEVEL_TURN_ON_INTERSTITIAL, value);
    }

    // SET COUNTER VARIABLE
    public static int DEFAULT_COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL = 2;

    public static int CounterNumbBetweenTwoInterstitial
    {
        get => PlayerPrefs.GetInt(Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL,
            DEFAULT_COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL);
        set => PlayerPrefs.SetInt(Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL, value);
    }

    // SET TIME TO ENABLE BETWEEN 2 INTERSTITIAL (ON WIN,LOSE,REPLAY GAME)
    public static int DEFAULT_SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL = 30;

    public static int TimeWinBetweenTwoInterstitial
    {
        get => PlayerPrefs.GetInt(Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL,
            DEFAULT_SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL);
        set => PlayerPrefs.SetInt(Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL, value);
    }

    // TOGGLE SHOW INTERSTITIAL ON LOSE GAME ? 0:NO, 1:YES
    public static int DEFAULT_SHOW_INTERSTITIAL_ON_LOSE_GAME = 0;

    public static int UseShowInterstitialOnLoseGame
    {
        get => PlayerPrefs.GetInt(Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME, DEFAULT_SHOW_INTERSTITIAL_ON_LOSE_GAME);
        set => PlayerPrefs.SetInt(Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME, value);
    }

    // SET TIME TO ENABLE BETWEEN 2 INTERSTITIAL (ON LOSE GAME)
    public static int DEFAULT_SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL = 45;

    public static int TimeLoseBetweenTwoInterstitial
    {
        get => PlayerPrefs.GetInt(Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL,
            DEFAULT_SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL);
        set => PlayerPrefs.SetInt(Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL, value);
    }

    public static int GetLogEventFirstStartLevel(int level)
    {
        return PlayerPrefs.GetInt($"First_Start_Level_{level}", 0);
    }

    public static void SetLogEventFirstStartLevel(int level, bool isOwned = true)
    {
        PlayerPrefs.SetInt($"First_Start_Level_{level}", isOwned ? 1 : 0);
    }

    #endregion
}

public static partial class Data
{
    private static bool GetBool(string key, bool defaultValue = false) =>
        PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) > 0;

    private static void SetBool(string id, bool value) => PlayerPrefs.SetInt(id, value ? 1 : 0);

    private static int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    private static void SetInt(string id, int value) => PlayerPrefs.SetInt(id, value);

    private static string GetString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
    private static void SetString(string id, string value) => PlayerPrefs.SetString(id, value);
}