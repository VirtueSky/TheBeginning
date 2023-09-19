using System;

public static class Observer
{
    #region GameSystem

    // Debug
    public static Action DebugChanged;

    // Level Spawn
    public static Action CurrentLevelChanged;

    // Setting
    public static Action MusicChanged;
    public static Action SoundChanged;

    public static Action VibrationChanged;

    // Ads

    // Other
    public static Action CoinMove;
    public static Action ClickButton;
    public static Action<string> TrackClickButton;
    public static Action PurchaseFail;
    public static Action PurchaseSucceed;
    public static Action ClaimReward;

    #endregion

    #region Gameplay

    // Game event
    public static Action<Level> StartLevel;
    public static Action<Level> ReplayLevel;
    public static Action<Level> SkipLevel;

    #endregion
}