#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;
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
            TrackingIosATT();
        }

        private async void TrackingIosATT()
        {
#if UNITY_IOS
            await UniTask.WaitUntil(() => firebaseDependencyAvailable.Value);
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking(AppTracking.FirebaseAnalyticTrackATTResult);
            }
#endif
        }
    }
}