using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.FirebaseTracking;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Banner Variable", fileName = "banner_ad_variable")]
public class BannerVariable : BaseSO
{
    [SerializeField] private AdUnitVariable bannerVariable;
    [Space, SerializeField] private BooleanVariable isOffBannerVariable;

    [Space, Header("Firebase Remote Config"), SerializeField]
    private BooleanVariable remoteConfigOnOffBanner;

    [Space, Header("Log Event Firebase Analytic"), SerializeField]
    private LogEventFirebaseNoParam logEventShowBanner;

    [SerializeField] private LogEventFirebaseNoParam logEventHideBanner;

    bool Condition()
    {
        return bannerVariable.IsReady() && !isOffBannerVariable.Value && remoteConfigOnOffBanner.Value;
    }

    public void Show()
    {
        if (Condition())
        {
            bannerVariable.Show();
            logEventShowBanner.LogEvent();
        }
    }

    public void HideBanner()
    {
        bannerVariable.Destroy();
        logEventHideBanner.LogEvent();
    }
}