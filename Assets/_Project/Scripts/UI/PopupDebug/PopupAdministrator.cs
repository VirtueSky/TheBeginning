using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class PopupAdministrator : UIPopup
{
    [HeaderLine("UI")] [SerializeField] private Toggle toggleOffUI;
    [SerializeField] private Toggle toggleIsTesting;
    [SerializeField] private Toggle toggleOffInterAds;
    [SerializeField] private Toggle toggleOffBannerAds;
    [SerializeField] private Toggle toggleOffRewardAds;
    [SerializeField] private TMP_InputField inputFieldCurrency;
    [SerializeField] private TMP_InputField inputFieldLevel;

    [HeaderLine("SO")] [SerializeField] private BooleanVariable isOffUIVariable;
    [SerializeField] private BooleanVariable isOffInterAds;
    [SerializeField] private BooleanVariable isOffBannerAds;
    [SerializeField] private BooleanVariable isOffRewardAds;
    [SerializeField] private BooleanVariable isTestingVariable;

    [SerializeField] private IntegerVariable indexLevelVariable;
    [SerializeField] private IntegerVariable currencyVariable;
    [SerializeField] private AdManagerVariable adManagerVariable;
    private bool isSetCurrency = false;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        isSetCurrency = false;
        SetupDefault();
    }

    void SetupDefault()
    {
        toggleOffUI.isOn = isOffUIVariable.Value;
        toggleIsTesting.isOn = isTestingVariable.Value;
        toggleOffBannerAds.isOn = isOffBannerAds.Value;
        toggleOffInterAds.isOn = isOffInterAds.Value;
        toggleOffRewardAds.isOn = isOffRewardAds.Value;
    }

    void Setup()
    {
        isOffUIVariable.Value = toggleOffUI.isOn;
        isTestingVariable.Value = toggleIsTesting.isOn;
        isOffInterAds.Value = toggleOffInterAds.isOn;
        isOffBannerAds.Value = toggleOffBannerAds.isOn;
        isOffRewardAds.Value = toggleOffRewardAds.isOn;

        if (inputFieldCurrency.text != "")
        {
            isSetCurrency = true;
            currencyVariable.Value = int.Parse(inputFieldCurrency.text);
        }

        if (inputFieldLevel.text != "")
        {
            indexLevelVariable.Value = int.Parse(inputFieldLevel.text);
        }

        inputFieldCurrency.text = "";
        inputFieldLevel.text = "";
    }

    public void OnClickAdd10000Coin()
    {
        currencyVariable.Value += 10000;
    }

    public void OnClickShowBanner()
    {
        adManagerVariable.Value.ShowBanner();
    }

    public void OnClickHideBanner()
    {
        adManagerVariable.Value.HideBanner();
    }

    public void OnClickShowInter()
    {
        adManagerVariable.Value.ShowInterstitial();
    }

    public void OnClickShowReward()
    {
        adManagerVariable.Value.ShowRewardAds();
    }

    public void OnClickUnlockAllSkins()
    {
    }

    public void OnClickOk()
    {
        Setup();
        if (isSetCurrency)
        {
            Tween.Delay(1.3f, () => { Hide(); });
        }
        else
        {
            Hide();
        }
    }
}