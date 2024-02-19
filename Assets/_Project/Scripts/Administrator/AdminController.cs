using TheBeginning.AppControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class AdminController : MonoBehaviour
{
    [HeaderLine("UI")] [SerializeField] private Button btnShowHideAdmin;
    [SerializeField] private Toggle toggleOffUI;
    [SerializeField] private Toggle toggleIsTesting;
    [SerializeField] private Toggle toggleOffInterAds;
    [SerializeField] private Toggle toggleOffBannerAds;
    [SerializeField] private Toggle toggleOffRewardAds;
    [SerializeField] private TMP_InputField inputFieldCurrency;
    [SerializeField] private TMP_InputField inputFieldLevel;
    [SerializeField] private Button btnJumpToLevel;
    [SerializeField] private Button btnEnterCurrency;
    [SerializeField] private Button btnAddCurrency;
    [SerializeField] private Button btnUnlockAllSkin;
    [SerializeField] private Button btnShowBanner;
    [SerializeField] private Button btnHideBanner;
    [SerializeField] private Button btnShowInter;
    [SerializeField] private Button btnShowReward;
    [SerializeField] private Button btnModifyConsent;
    [HeaderLine("SO")] [SerializeField] private BooleanVariable isOffUIVariable;
    [SerializeField] private BooleanVariable isOffInterAds;
    [SerializeField] private BooleanVariable isOffBannerAds;
    [SerializeField] private BooleanVariable isOffRewardAds;
    [SerializeField] private BooleanVariable isTestingVariable;
    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private IntegerVariable currencyVariable;
    [SerializeField] private ItemConfig itemConfig;
    [SerializeField] private EventNoParam showConsentOption;
    [SerializeField] private BooleanVariable gdpr_required_variable;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;

    private void OnEnable()
    {
        SetupDefault();
        btnModifyConsent.gameObject.SetActive(gdpr_required_variable.Value);
        // button
        btnModifyConsent.onClick.AddListener(OnClickModifyConsent);
        btnJumpToLevel.onClick.AddListener(OnClickJumpToLevel);
        btnEnterCurrency.onClick.AddListener(OnClickEnterCurrency);
        btnAddCurrency.onClick.AddListener(OnClickAdd10000Coin);
        btnUnlockAllSkin.onClick.AddListener(OnClickUnlockAllSkins);
        btnShowBanner.onClick.AddListener(OnClickShowBanner);
        btnHideBanner.onClick.AddListener(OnClickHideBanner);
        btnShowInter.onClick.AddListener(OnClickShowInter);
        btnShowReward.onClick.AddListener(OnClickShowReward);
        // toggle
        toggleOffUI.onValueChanged.AddListener(OnChangeOffUI);
        toggleIsTesting.onValueChanged.AddListener(OnChangeOffIsTesting);
        toggleOffBannerAds.onValueChanged.AddListener(OnChangeOffBanner);
        toggleOffInterAds.onValueChanged.AddListener(OnChangeOffInter);
        toggleOffRewardAds.onValueChanged.AddListener(OnChangeOffReward);
    }

    private void OnDisable()
    {
        // button
        btnModifyConsent.onClick.RemoveListener(OnClickModifyConsent);
        btnJumpToLevel.onClick.RemoveListener(OnClickJumpToLevel);
        btnEnterCurrency.onClick.RemoveListener(OnClickEnterCurrency);
        btnAddCurrency.onClick.RemoveListener(OnClickAdd10000Coin);
        btnUnlockAllSkin.onClick.RemoveListener(OnClickUnlockAllSkins);
        btnShowBanner.onClick.RemoveListener(OnClickShowBanner);
        btnHideBanner.onClick.RemoveListener(OnClickHideBanner);
        btnShowInter.onClick.RemoveListener(OnClickShowInter);
        btnShowReward.onClick.RemoveListener(OnClickShowReward);
        // toggle
        toggleOffUI.onValueChanged.RemoveListener(OnChangeOffUI);
        toggleIsTesting.onValueChanged.RemoveListener(OnChangeOffIsTesting);
        toggleOffBannerAds.onValueChanged.RemoveListener(OnChangeOffBanner);
        toggleOffInterAds.onValueChanged.RemoveListener(OnChangeOffInter);
        toggleOffRewardAds.onValueChanged.RemoveListener(OnChangeOffReward);
    }

    void SetupDefault()
    {
        toggleOffUI.isOn = isOffUIVariable.Value;
        toggleIsTesting.isOn = isTestingVariable.Value;
        toggleOffBannerAds.isOn = isOffBannerAds.Value;
        toggleOffInterAds.isOn = isOffInterAds.Value;
        toggleOffRewardAds.isOn = isOffRewardAds.Value;
    }

    public void OnClickJumpToLevel()
    {
        if (inputFieldLevel.text != "")
        {
            indexLevelVariable.Value = int.Parse(inputFieldLevel.text);
        }

        inputFieldLevel.text = "";
        callPlayCurrentLevelEvent.Raise();
    }

    public void OnClickEnterCurrency()
    {
        if (inputFieldCurrency.text != "")
        {
            currencyVariable.Value = int.Parse(inputFieldCurrency.text);
        }

        inputFieldCurrency.text = "";
    }

    public void OnClickAdd10000Coin()
    {
        currencyVariable.Value += 10000;
    }

    public void OnClickShowBanner()
    {
        AppControlAds.ShowBanner();
    }

    public void OnClickHideBanner()
    {
        AppControlAds.HideBanner();
    }

    public void OnClickShowInter()
    {
        AppControlAds.ShowInterstitial();
    }

    public void OnClickShowReward()
    {
        AppControlAds.ShowReward();
    }

    public void OnClickUnlockAllSkins()
    {
        itemConfig.UnlockAllSkins();
    }

    public void OnChangeOffIsTesting(bool isOn)
    {
        isTestingVariable.Value = isOn;
    }

    public void OnChangeOffInter(bool isOn)
    {
        isOffInterAds.Value = isOn;
    }

    public void OnChangeOffReward(bool isOn)
    {
        isOffRewardAds.Value = isOn;
    }

    public void OnChangeOffBanner(bool isOn)
    {
        isOffBannerAds.Value = isOn;
    }

    public void OnChangeOffUI(bool isOn)
    {
        isOffUIVariable.Value = isOn;
    }

    public void OnClickModifyConsent()
    {
        showConsentOption.Raise();
    }

    public void OnClickShowHideAdmin()
    {
    }
}