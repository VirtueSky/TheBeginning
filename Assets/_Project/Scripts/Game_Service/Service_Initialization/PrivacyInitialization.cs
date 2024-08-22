#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        }

        private async void RequestTracking()
        {
#if UNITY_IOS
            await UniTask.WaitUntil(() => firebaseDependencyAvailable.Value);
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
            AppTracking.StartTrackingAdjust();
            AppTracking.StartTrackingAppsFlyer();
            AppTracking.FirebaseAnalyticTrackATTResult(status);
        }
    }
}