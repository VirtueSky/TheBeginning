using System;
using TheBeginning.Custom_Scriptable_Event;
using UnityEngine;
using VirtueSky.Events;
#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif

public class FirebaseAnalyticManager : MonoBehaviour
{
    [SerializeField] private TrackingFirebaseHasParamEvent trackingFirebaseHasParamEvent;
    [SerializeField] private StringEvent trackingFirebaseEventNoParam;

    private void OnEnable()
    {
        if (trackingFirebaseEventNoParam != null)
        {
            trackingFirebaseEventNoParam.AddListener(LogEvent);
        }

        if (trackingFirebaseHasParamEvent != null)
        {
            trackingFirebaseHasParamEvent.AddListener(LogEvent);
        }
    }

    private void OnDisable()
    {
        if (trackingFirebaseEventNoParam != null)
        {
            trackingFirebaseEventNoParam.RemoveListener(LogEvent);
        }

        if (trackingFirebaseHasParamEvent != null)
        {
            trackingFirebaseHasParamEvent.RemoveListener(LogEvent);
        }
    }

    public void InitFirebaseAnalytics()
    {
#if VIRTUESKY_FIREBASE_ANALYTIC
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
#endif
    }

//     #region Tracking Gameplay
//
//     public void OnStartLevel(Level level)
//     {
//         MethodBase function = MethodBase.GetCurrentMethod();
// #if VIRTUESKY_FIREBASE_ANALYTIC
//         LogEvent(function.Name, "level_name", level.gameObject.name);
// #endif
//     }
//
//     public void OnLoseLevel(Level level)
//     {
//         MethodBase function = MethodBase.GetCurrentMethod();
// #if VIRTUESKY_FIREBASE_ANALYTIC
//         LogEvent(function.Name, "level_name", level.gameObject.name);
// #endif
//     }
//
//     public void OnWinLevel(Level level)
//     {
//         MethodBase function = MethodBase.GetCurrentMethod();
// #if VIRTUESKY_FIREBASE_ANALYTIC
//         LogEvent(function.Name, "level_name", level.gameObject.name);
// #endif
//     }
//
//     public void OnReplayLevel(Level level)
//     {
//         MethodBase function = MethodBase.GetCurrentMethod();
// #if VIRTUESKY_FIREBASE_ANALYTIC
//         LogEvent(function.Name, "level_name", level.gameObject.name);
// #endif
//     }
//
//     #endregion

    #region Function Base

    public static bool IsMobile()
    {
        return (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer);
    }

    public static void LogEvent(string eventName, Parameter[] parameters)
    {
#if VIRTUESKY_FIREBASE_ANALYTIC
        if (!IsMobile()) return;

        try
        {
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
#endif
    }

    public static void LogEvent(TrackingFirebaseEventHasParamData trackingFirebaseEventHasParamData)
    {
#if VIRTUESKY_FIREBASE_ANALYTIC
        if (!IsMobile()) return;

        try
        {
            Parameter[] parameters =
            {
                new Parameter(trackingFirebaseEventHasParamData.parameterName,
                    trackingFirebaseEventHasParamData.parameterValue)
            };

            FirebaseAnalytics.LogEvent(trackingFirebaseEventHasParamData.eventName, parameters);
        }

        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
#endif
    }

    public static void LogEvent(string eventName)
    {
        if (!IsMobile()) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
        try
        {
            FirebaseAnalytics.LogEvent(eventName);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
#endif
    }

    #endregion
}