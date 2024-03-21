using TheBeginning.AppControl;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class RequireInternetInitialization : ServiceInitialization
    {
        [SerializeField] private GameConfig gameConfig;

        public override void Initialization()
        {
            if (gameConfig.enableRequireInternet)
            {
                InvokeRepeating(nameof(RequireInternet), gameConfig.timeDelayCheckInternet,
                    gameConfig.timeLoopCheckInternet);
            }
        }

        void RequireInternet()
        {
            Common.CheckInternetConnection(() =>
                {
                    if (AppControlPopup.IsPopupReady<PopupRequireInternet>())
                    {
                        AppControlPopup.Hide<PopupRequireInternet>();
                    }
                },
                () => { AppControlPopup.Show<PopupRequireInternet>(false); });
        }
    }
}