using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Localization;
#if ADS_ADMOB
using GoogleMobileAds.Ump.Api;
#endif

namespace TheBeginning.UI
{
    public class PopupSetting : UIPopup
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
#if ADS_ADMOB
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
            btnLanguageEnglish.effectFactor = 1;
            btnLanguageVietNam.effectFactor = 1;
            if (Locale.CurrentLanguage == Language.English) btnLanguageEnglish.effectFactor = 0;
            if (Locale.CurrentLanguage == Language.Vietnamese) btnLanguageVietNam.effectFactor = 0;
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