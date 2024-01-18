using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Events;
using System.Threading.Tasks;
using VirtueSky.Inspector;
#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif
#if VIRTUESKY_FIREBASE
using Firebase.Extensions;
#endif

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    [SerializeField] EventNoParam fetchRemoteConfigCompleted;
    [ReadOnly, SerializeField] private List<RemoteConfigData> listRemoteConfigData;

    public async void InitFirebaseRemoteConfig()
    {
        var defaults = new System.Collections.Generic.Dictionary<string, object>
        {
            { Constant.USE_LEVEL_AB_TESTING, Data.DEFAULT_USE_LEVEL_AB_TESTING },
            { Constant.LEVEL_TURN_ON_INTERSTITIAL, Data.DEFAULT_LEVEL_TURN_ON_INTERSTITIAL },
            {
                Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL,
                Data.DEFAULT_COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL
            },
            {
                Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL,
                Data.DEFAULT_SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL
            },
            {
                Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME, Data.DEFAULT_SHOW_INTERSTITIAL_ON_LOSE_GAME
            },
            {
                Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL,
                Data.DEFAULT_SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL
            },
        };
#if VIRTUESKY_FIREBASE_REMOTECONFIG
        await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                // [END set_defaults]
                Debug.Log("RemoteConfig configured and ready!");
            });

        await FetchDataAsync();
#endif
    }

    public Task FetchDataAsync()
    {
#if VIRTUESKY_FIREBASE_REMOTECONFIG
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
                        Debug.Log(String.Format(
                            "Remote data loaded and ready (last fetch time {0}).",
                            info.FetchTime));
                    });

                Data.UseLevelABTesting = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.USE_LEVEL_AB_TESTING).StringValue);
                Data.LevelTurnOnInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.LEVEL_TURN_ON_INTERSTITIAL).StringValue);
                Data.CounterNumbBetweenTwoInterstitial =
                    int.Parse(FirebaseRemoteConfig.DefaultInstance
                        .GetValue(Constant.COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL).StringValue);
                Data.TimeWinBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL).StringValue);
                Data.UseShowInterstitialOnLoseGame = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SHOW_INTERSTITIAL_ON_LOSE_GAME).StringValue);
                Data.TimeLoseBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL).StringValue);

                Debug.Log("<color=Green>Firebase Remote Config Fetching Values</color>");
                Debug.Log($"<color=Green>Data.UseLevelABTesting: {Data.UseLevelABTesting}</color>");
                Debug.Log(
                    $"<color=Green>Data.LevelTurnOnInterstitial: {Data.LevelTurnOnInterstitial}</color>");
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

            fetchRemoteConfigCompleted.Raise();
        });
#endif
    }
}

[Serializable]
public class RemoteConfigData
{
    public string key;
    public object value;
}