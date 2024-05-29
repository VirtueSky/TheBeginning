using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Core;
using VirtueSky.FirebaseTracking;
using VirtueSky.Inspector;
using VirtueSky.Variables;

[CreateAssetMenu(menuName = "Ads Variable/Banner Variable", fileName = "banner_ad_variable")]
public class BannerVariable : BaseSO
{
    [SerializeField] private AdSetting adSetting;
    [SerializeField] private AdUnitVariable bannerVariable;
    [Space, SerializeField] private BooleanVariable isOffBannerVariable;

    [Space, HeaderLine("Firebase Remote Config"), SerializeField]
    private BooleanVariable remoteConfigOnOffBanner;

    [Space, HeaderLine("Log Event Firebase Analytic"), SerializeField]
    private LogEventFirebaseNoParam logEventShowBanner;

    [SerializeField] private LogEventFirebaseNoParam logEventHideBanner;
    public AdUnitVariable AdUnitBannerVariable => bannerVariable;

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

    public void Hide()
    {
        switch (adSetting.CurrentAdNetwork)
        {
            case AdNetwork.Max:
                (bannerVariable as MaxBannerVariable)?.Hide();
                break;
            case AdNetwork.Admob:
                (bannerVariable as AdmobBannerVariable)?.Hide();
                break;
        }

        logEventHideBanner.LogEvent();
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