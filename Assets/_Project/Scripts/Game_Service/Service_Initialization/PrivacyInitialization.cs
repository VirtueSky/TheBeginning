#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using Cysharp.Threading.Tasks;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Tracking;
using VirtueSky.Variables;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class PrivacyInitialization : ServiceInitialization
    {
        [SerializeField] private BooleanVariable firebaseDependencyAvailable;

        public override void Initialization()
        {
            RequestTracking();
            GetInfo();
        }

        private void RequestTracking()
        {
#if UNITY_IOS
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking(CallbackTracking);
            }
            else
            {
                AppTracking.StartTrackingAdjust();
                AppTracking.StartTrackingAppsFlyer();
            }
#else
            AppTracking.StartTrackingAdjust();
            AppTracking.StartTrackingAppsFlyer();
#endif
        }

        private void CallbackTracking(int status)
        {
            App.RunOnMainThread(() =>
            {
                AppTracking.StartTrackingAdjust();
                AppTracking.StartTrackingAppsFlyer();
                TrackAttFirebase(status);
            });
        }

        async void TrackAttFirebase(int status)
        {
            await UniTask.WaitUntil(() => firebaseDependencyAvailable.Value);
            AppTracking.FirebaseAnalyticTrackATTResult(status);
        }

        void GetInfo()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            Debug.Log($"DeviceID: {deviceId}");
            Application.RequestAdvertisingIdentifierAsync((string advertisingId, bool trackingEnable, string error) =>
            {
                if (!string.IsNullOrEmpty(advertisingId))
                {
                    Debug.Log($"AdvertisingId: {advertisingId}");
                }
                else
                {
                    Debug.Log("Failed to get AdvertisingId");
                }

                if (!trackingEnable)
                {
                    Debug.Log("User has limited ad tracking");
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.Log($"Error when get AdvertisingId: {error}");
                }
            });
        }
    }
}