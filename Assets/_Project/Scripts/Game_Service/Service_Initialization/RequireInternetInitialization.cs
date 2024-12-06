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
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private BooleanEvent showRequireInternetEvent;

        public override void Initialization()
        {
            if (gameSettings.EnableRequireInternet)
            {
                InvokeRepeating(nameof(RequireInternet), gameSettings.TimeDelayCheckInternet,
                    gameSettings.TimeLoopCheckInternet);
            }
        }

        void RequireInternet()
        {
            Common.CheckInternetConnection(() => { showRequireInternetEvent.Raise(false); },
                () => { showRequireInternetEvent.Raise(true); });
        }
    }
}