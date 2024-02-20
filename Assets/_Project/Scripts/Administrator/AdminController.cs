using System;
using PrimeTween;
using TheBeginning.AppControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class AdminController : MonoBehaviour
{
    [HeaderLine("UI")] [ReadOnly, SerializeField]
    private bool isShow = false;

    [SerializeField] private RectTransform container;
    [SerializeField] private GameObject holder;
    [SerializeField] private Button btnShowHideAdmin;
    [SerializeField] private Sprite iconBtnShow;
    [SerializeField] private Sprite iconBtnHide;
    [SerializeField] private Toggle toggleOffUI;
    [SerializeField] private Toggle toggleIsTesting;
    [SerializeField] private Toggle toggleOffInterAds;
    [SerializeField] private Toggle toggleOffBannerAds;
    [SerializeField] private Toggle toggleOffRewardAds;
    [SerializeField] private TMP_InputField inputFieldCurrency;
    [SerializeField] private TMP_InputField inputFieldLevel;
    [SerializeField] private Button btnNextLevel;
    [SerializeField] private Button btnPrevLevel;
    [SerializeField] private Button btnWinLevel;
    [SerializeField] private Button btnLoseLevel;
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
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private EventNoParam showConsentOption;
    [SerializeField] private BooleanVariable gdpr_required_variable;
    [SerializeField] private EventNoParam callPlayCurrentLevelEvent;
    [SerializeField] private GameStateVariable gameStateVariable;
    [SerializeField] private EventNoParam callNextLevelEvent;
    [SerializeField] private EventNoParam callPreviousLevelEvent;
    [SerializeField] private FloatEvent callWinLevelEvent;
    [SerializeField] private FloatEvent callLoseLevelEvent;

    private void Awake()
    {
        gameObject.SetActive(gameConfig.enableAdministrator);
    }

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
        btnShowHideAdmin.onClick.AddListener(OnClickShowHideAdmin);
        btnNextLevel.onClick.AddListener(OnClickNextLevel);
        btnPrevLevel.onClick.AddListener(OnClickPreviousLevel);
        btnWinLevel.onClick.AddListener(OnClickWinLevel);
        btnLoseLevel.onClick.AddListener(OnClickLoseLevel);
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
        btnShowHideAdmin.onClick.RemoveListener(OnClickShowHideAdmin);
        btnNextLevel.onClick.RemoveListener(OnClickNextLevel);
        btnPrevLevel.onClick.RemoveListener(OnClickPreviousLevel);
        btnWinLevel.onClick.RemoveListener(OnClickWinLevel);
        btnLoseLevel.onClick.RemoveListener(OnClickLoseLevel);
        // toggle
        toggleOffUI.onValueChanged.RemoveListener(OnChangeOffUI);
        toggleIsTesting.onValueChanged.RemoveListener(OnChangeOffIsTesting);
        toggleOffBannerAds.onValueChanged.RemoveListener(OnChangeOffBanner);
        toggleOffInterAds.onValueChanged.RemoveListener(OnChangeOffInter);
        toggleOffRewardAds.onValueChanged.RemoveListener(OnChangeOffReward);
    }

    void SetupDefault()
    {
        Hide();
        Refresh();
    }

    void Refresh()
    {
        toggleOffUI.isOn = isOffUIVariable.Value;
        toggleIsTesting.isOn = isTestingVariable.Value;
        toggleOffBannerAds.isOn = isOffBannerAds.Value;
        toggleOffInterAds.isOn = isOffInterAds.Value;
        toggleOffRewardAds.isOn = isOffRewardAds.Value;

        btnShowBanner.gameObject.SetActive(Application.isMobilePlatform);
        btnHideBanner.gameObject.SetActive(Application.isMobilePlatform);
        btnShowInter.gameObject.SetActive(Application.isMobilePlatform);
        btnShowReward.gameObject.SetActive(Application.isMobilePlatform);

        btnNextLevel.gameObject.SetActive(gameStateVariable.Value == GameState.PlayingGame);
        btnPrevLevel.gameObject.SetActive(gameStateVariable.Value == GameState.PlayingGame);
        btnWinLevel.gameObject.SetActive(gameStateVariable.Value == GameState.PlayingGame);
        btnLoseLevel.gameObject.SetActive(gameStateVariable.Value == GameState.PlayingGame);
    }

     void OnClickJumpToLevel()
    {
        if (inputFieldLevel.text != "")
        {
            indexLevelVariable.Value = int.Parse(inputFieldLevel.text);
        }

        inputFieldLevel.text = "";
        callPlayCurrentLevelEvent.Raise();
    }

     void OnClickEnterCurrency()
    {
        if (inputFieldCurrency.text != "")
        {
            currencyVariable.Value = int.Parse(inputFieldCurrency.text);
        }

        inputFieldCurrency.text = "";
    }

     void OnClickAdd10000Coin()
    {
        currencyVariable.Value += 10000;
    }

     void OnClickShowBanner()
    {
        AppControlAds.ShowBanner();
    }

     void OnClickHideBanner()
    {
        AppControlAds.HideBanner();
    }

     void OnClickShowInter()
    {
        AppControlAds.ShowInterstitial();
    }

     void OnClickShowReward()
    {
        AppControlAds.ShowReward();
    }

     void OnClickUnlockAllSkins()
    {
        itemConfig.UnlockAllSkins();
    }

    void OnClickNextLevel()
    {
        callNextLevelEvent.Raise();
    }

    void OnClickPreviousLevel()
    {
        callPreviousLevelEvent.Raise();
    }

    void OnClickWinLevel()
    {
        callWinLevelEvent.Raise(1.5f);
    }

    void OnClickLoseLevel()
    {
        callLoseLevelEvent.Raise(1.5f);
    }

     void OnChangeOffIsTesting(bool isOn)
    {
        isTestingVariable.Value = isOn;
    }

     void OnChangeOffInter(bool isOn)
    {
        isOffInterAds.Value = isOn;
    }

     void OnChangeOffReward(bool isOn)
    {
        isOffRewardAds.Value = isOn;
    }

     void OnChangeOffBanner(bool isOn)
    {
        isOffBannerAds.Value = isOn;
    }

     void OnChangeOffUI(bool isOn)
    {
        isOffUIVariable.Value = isOn;
    }

     void OnClickModifyConsent()
    {
        showConsentOption.Raise();
    }

     void OnClickShowHideAdmin()
    {
        if (isShow)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Show()
    {
        isShow = true;
        Refresh();
        holder.gameObject.SetActive(true);
        Tween.UIAnchoredPositionX(container, 550, .25f).OnComplete(() =>
        {
            btnShowHideAdmin.GetComponent<Image>().sprite = iconBtnHide;
        });
    }

    void Hide()
    {
        isShow = false;
        Tween.UIAnchoredPositionX(container, 0, .25f).OnComplete(() =>
        {
            holder.gameObject.SetActive(false);
            btnShowHideAdmin.GetComponent<Image>().sprite = iconBtnShow;
        });
    }
}