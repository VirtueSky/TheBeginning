using System;
using System.Reflection;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Core;
using VirtueSky.Events;

public class FirebaseManager : BaseMono
{
    public DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public EventNoParam initFirebaseSuccess;

    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #region FirebaseInitGetRemoteConfig

    public override void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();

                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private async void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        var defaults = new System.Collections.Generic.Dictionary<string, object>
        {
            { Constant.USE_LEVEL_AB_TESTING, Data.DEFAULT_USE_LEVEL_AB_TESTING },
            { Constant.LEVEL_TURN_ON_INTERSTITIAL, Data.DEFAULT_LEVEL_TURN_ON_INTERSTITIAL },
            { Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL, Data.DEFAULT_COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL },
            { Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL, Data.DEFAULT_SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL },
            { Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME, Data.DEFAULT_SHOW_INTERSTITIAL_ON_LOSE_GAME },
            {
                Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL, Data.DEFAULT_SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL
            },
        };
        await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                // [END set_defaults]
                Debug.Log("RemoteConfig configured and ready!");
            });

        await FetchDataAsync();
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        return fetchTask.ContinueWithOnMainThread(tast =>
        {
            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            //SET NEW DATA FROM REMOTE CONFIG
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                            info.FetchTime));
                    });

                Data.UseLevelABTesting = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.USE_LEVEL_AB_TESTING).StringValue);
                Data.LevelTurnOnInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.LEVEL_TURN_ON_INTERSTITIAL).StringValue);
                Data.CounterNumbBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL).StringValue);
                Data.TimeWinBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL).StringValue);
                Data.UseShowInterstitialOnLoseGame = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME).StringValue);
                Data.TimeLoseBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL).StringValue);

                Debug.Log("<color=Green>Firebase Remote Config Fetching Values</color>");
                Debug.Log($"<color=Green>Data.UseLevelABTesting: {Data.UseLevelABTesting}</color>");
                Debug.Log($"<color=Green>Data.LevelTurnOnInterstitial: {Data.LevelTurnOnInterstitial}</color>");
                Debug.Log(
                    $"<color=Green>Data.CounterNumbBetweenTwoInterstitial: {Data.CounterNumbBetweenTwoInterstitial}</color>");
                Debug.Log(
                    $"<color=Green>Data.TimeWinBetweenTwoInterstitial: {Data.TimeWinBetweenTwoInterstitial}</color>");
                Debug.Log(
                    $"<color=Green>Data.UseShowInterstitialOnLoseGame: {Data.UseShowInterstitialOnLoseGame}</color>");
                Debug.Log(
                    $"<color=Green>Data.TimeLoseBetweenTwoInterstitial: {Data.TimeLoseBetweenTwoInterstitial}</color>");
                Debug.Log("<color=Green>Firebase Remote Config Fetching completed!</color>");
            }
            else
            {
                Debug.Log("Fetching data did not completed!");
            }

            initFirebaseSuccess.Raise();
        });
    }

    #endregion

    #region TrackingGameplay

    public void OnStartLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    public void OnLoseLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    public void OnWinLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    public void OnReplayLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    #endregion

    #region TrackingGameSystem

    public void OnRequestInterstitial()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public void OnShowInterstitialCompleted()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public void OnRequestReward()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public void OnShowRewardCompleted()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public void OnRequestBanner()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public void OnShowBanner()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    #endregion

    #region BaseLogFunction

    public static bool IsMobile()
    {
        return (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer);
    }

    public static void LogEvent(string paramName, Parameter[] parameters)
    {
        if (!IsMobile()) return;
        try
        {
            FirebaseAnalytics.LogEvent(paramName, parameters);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
    }

    public static void LogEvent(string paramName)
    {
        if (!IsMobile()) return;
        try
        {
            FirebaseAnalytics.LogEvent(paramName);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
    }

    #endregion
}