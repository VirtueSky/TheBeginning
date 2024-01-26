using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Events;
using System.Threading.Tasks;
using VirtueSky.Inspector;
using VirtueSky.Variables;
#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif
#if VIRTUESKY_FIREBASE
using Firebase.Extensions;
#endif

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    [SerializeField] private EventNoParam fetchRemoteConfigCompleted;
    [SerializeField] private List<RemoteConfigData> listRemoteConfigData;

    public void InitFirebaseRemoteConfig()
    {
#if VIRTUESKY_FIREBASE_REMOTECONFIG
        FetchDataAsync();
#endif
    }


    public Task FetchDataAsync()
    {
#if VIRTUESKY_FIREBASE_REMOTECONFIG
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
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
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                    task =>
                    {
                        Debug.Log(String.Format(
                            "Remote data loaded and ready (last fetch time {0}).",
                            info.FetchTime));
                        foreach (var remoteConfigData in listRemoteConfigData)
                        {
                            remoteConfigData.SetUpData(FirebaseRemoteConfig.DefaultInstance
                                .GetValue(remoteConfigData.key));
                        }

                        fetchRemoteConfigCompleted.Raise();
                    });

                Debug.Log("<color=Green>Firebase Remote Config Fetching completed!</color>");
            }
            else
            {
                Debug.Log("Fetching data did not completed!");
            }
        });
#endif
    }
}

[Serializable]
public class RemoteConfigData
{
    public string key;
    public TypeRemoteConfigData typeRemoteConfigData;

    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)]
    public StringVariable stringValue;

    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)] [ReadOnly]
    public string resultStringValue;


    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)]
    public BooleanVariable boolValue;

    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)] [ReadOnly]
    public bool resultBoolValue;


    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)]
    public IntegerVariable intValue;

    [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)] [ReadOnly]
    public int resultIntValue;

    public void SetUpData(ConfigValue result)
    {
        switch (typeRemoteConfigData)
        {
            case TypeRemoteConfigData.StringData:
                stringValue.Value = result.StringValue;
                resultStringValue = result.StringValue;
                break;
            case TypeRemoteConfigData.BooleanData:
                boolValue.Value = result.BooleanValue;
                resultBoolValue = result.BooleanValue;
                break;
            case TypeRemoteConfigData.IntData:
                intValue.Value = int.Parse(result.StringValue);
                resultIntValue = int.Parse(result.StringValue);
                break;
        }

        Debug.Log($"<color=Green>{key}: {result.StringValue}</color>");
    }
}

public enum TypeRemoteConfigData
{
    StringData,
    BooleanData,
    IntData
}