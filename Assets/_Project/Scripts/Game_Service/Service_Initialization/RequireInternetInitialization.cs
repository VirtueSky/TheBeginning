using TheBeginning.Config;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class RequireInternetInitialization : ServiceInitialization
    {
        [SerializeField] private BooleanEvent showRequireInternetEvent;

        public override void Initialization()
        {
            if (GameConfig.Instance.enableRequireInternet)
            {
                InvokeRepeating(nameof(RequireInternet), GameConfig.Instance.timeDelayCheckInternet,
                    GameConfig.Instance.timeLoopCheckInternet);
            }
        }

        void RequireInternet()
        {
            Common.CheckInternetConnection(() => { showRequireInternetEvent.Raise(false); },
                () => { showRequireInternetEvent.Raise(true); });
        }
    }
}