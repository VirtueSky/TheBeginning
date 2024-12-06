using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Localization;
#if VIRTUESKY_ADMOB
using GoogleMobileAds.Ump.Api;
#endif

namespace TheBeginning.UI
{
    public class SettingPopup : UIPopup
    {
        [SerializeField] private Button btnRestorePurchase;

        [SerializeField] private Button btnShowPrivacyConsent;

        [SerializeField] private Coffee.UIEffects.UIEffect btnLanguageEnglish;
        [SerializeField] private Coffee.UIEffects.UIEffect btnLanguageVietNam;
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
#if VIRTUESKY_ADMOB
            btnShowPrivacyConsent.gameObject.SetActive(ConsentInformation.PrivacyOptionsRequirementStatus ==
                                                       PrivacyOptionsRequirementStatus.Required);
#endif
            InitBtnLanguage();
        }

        protected override void OnBeforeHide()
        {
            base.OnBeforeHide();
            btnRestorePurchase.onClick.RemoveListener(OnClickRestorePurchase);
            btnShowPrivacyConsent.onClick.RemoveListener(OnClickShowPrivacyConsent);
        }

        void InitBtnLanguage()
        {
            btnLanguageEnglish.toneIntensity = 1;
            btnLanguageVietNam.toneIntensity = 1;
            if (Locale.CurrentLanguage == Language.English) btnLanguageEnglish.toneIntensity = 0;
            if (Locale.CurrentLanguage == Language.Vietnamese) btnLanguageVietNam.toneIntensity = 0;
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

        public void OnClickLanguageEn()
        {
            Locale.CurrentLanguage = Language.English;
            InitBtnLanguage();
        }

        public void OnClickLanguageVi()
        {
            Locale.CurrentLanguage = Language.Vietnamese;
            InitBtnLanguage();
        }
    }
}