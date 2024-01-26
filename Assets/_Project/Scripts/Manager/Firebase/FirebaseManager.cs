#if VIRTUESKY_FIREBASE
using Firebase;
using Firebase.Extensions;
#endif
using UnityEngine;
using VirtueSky.Core;

public class FirebaseManager : BaseMono
{
#if VIRTUESKY_FIREBASE
    public DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif

    [SerializeField] private FirebaseRemoteConfigManager firebaseRemoteConfigManager;
    [SerializeField] private FirebaseAnalyticManager firebaseAnalyticManager;


    public void Start()
    {
#if VIRTUESKY_FIREBASE
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                firebaseRemoteConfigManager.InitFirebaseRemoteConfig();
                firebaseAnalyticManager.InitFirebaseAnalytics();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
#endif
    }
}