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
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private BooleanEvent showRequireInternetEvent;

        public override void Initialization()
        {
            if (gameConfig.EnableRequireInternet)
            {
                InvokeRepeating(nameof(RequireInternet), gameConfig.TimeDelayCheckInternet,
                    gameConfig.TimeLoopCheckInternet);
            }
        }

        void RequireInternet()
        {
            Common.CheckInternetConnection(() => { showRequireInternetEvent.Raise(false); },
                () => { showRequireInternetEvent.Raise(true); });
        }
    }
}