using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
#if ADS_ADMOB
using GoogleMobileAds.Ump.Api;
#endif

public class PopupSetting : UIPopup
{
    [SerializeField] private Button btnRestorePurchase;
    [SerializeField] private Button btnShowPrivacyConsent;
    [SerializeField] private EventNoParam restorePurchaseEvent;
    [SerializeField] private EventNoParam callShowAgainGDPREvent;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        btnRestorePurchase.onClick.AddListener(OnClickRestorePurchase);
        btnShowPrivacyConsent.onClick.AddListener(OnClickShowPrivacyConsent);
        SetupButtonDefault();
#if UNITY_IOS
        btnRestorePurchase.gameObject.SetActive(true);
#endif
#if ADS_ADMOB
        btnShowPrivacyConsent.gameObject.SetActive(ConsentInformation.PrivacyOptionsRequirementStatus ==
                                                   PrivacyOptionsRequirementStatus.Required);
#endif
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
        btnRestorePurchase.onClick.RemoveListener(OnClickRestorePurchase);
        btnShowPrivacyConsent.onClick.RemoveListener(OnClickShowPrivacyConsent);
    }

    void SetupButtonDefault()
    {
        btnRestorePurchase.gameObject.SetActive(false);
        btnShowPrivacyConsent.gameObject.SetActive(false);
    }

    public void OnClickRestorePurchase()
    {
        restorePurchaseEvent.Raise();
    }

    public void OnClickShowPrivacyConsent()
    {
        callShowAgainGDPREvent.Raise();
    }
}