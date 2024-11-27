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
            if (GameConfig.EnableRequireInternet)
            {
                InvokeRepeating(nameof(RequireInternet), GameConfig.TimeDelayCheckInternet,
                    GameConfig.TimeLoopCheckInternet);
            }
        }

        void RequireInternet()
        {
            Common.CheckInternetConnection(() => { showRequireInternetEvent.Raise(false); },
                () => { showRequireInternetEvent.Raise(true); });
        }
    }
}