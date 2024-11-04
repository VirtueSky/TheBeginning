using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.Tracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Banner Variable", fileName = "banner_ad_variable")]
public class BannerAdVariable : AdVariable
{
    [SerializeField] private AdUnitVariable bannerVariable;
    [Space, SerializeField] private BooleanVariable isOffBannerVariable;

    [Space, HeaderLine("Firebase Remote Config"), SerializeField]
    private BooleanVariable remoteConfigOnOffBanner;

    [Space, HeaderLine("Track Firebase Analytic"), SerializeField]
    private TrackingFirebaseNoParam trackingFirebaseShowBanner;

    [SerializeField] private TrackingFirebaseNoParam trackingFirebaseHideBanner;
    public AdUnitVariable AdUnitBannerVariable => bannerVariable;

    public override void Init()
    {
    }

    bool Condition()
    {
        return bannerVariable.IsReady() && !isOffBannerVariable.Value && remoteConfigOnOffBanner.Value;
    }

    public void Show()
    {
        if (Condition())
        {
            bannerVariable.Show();
            trackingFirebaseShowBanner.TrackEvent();
        }
    }

    public void Hide()
    {
        bannerVariable.HideBanner();
        trackingFirebaseHideBanner.TrackEvent();
    }

    public AdUnitVariable ShowNoCondition()
    {
        return bannerVariable.Show();
    }

    public void Destroy()
    {
        bannerVariable.Destroy();
    }
}