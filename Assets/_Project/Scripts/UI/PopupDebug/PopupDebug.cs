using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.Variables;

public class PopupDebug : UIPopup
{
    public TMP_InputField SetLevel;
    public TMP_InputField SetCoin;
    public Toggle ToggleTesting;

    public Toggle ToggleIsOffInterAds;

    [SerializeField] private EventNoParam prepareLevelEvent;
    [SerializeField] private IntegerVariable indexLevelVariable;

    [SerializeField] private IntegerVariable currencyTotalVariable;

    [SerializeField] private BooleanVariable isTestingVariable;
    [SerializeField] private BooleanVariable isOffInterAdsVariable;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        ToggleTesting.isOn = isTestingVariable.Value;
        ToggleIsOffInterAds.isOn = isOffInterAdsVariable.Value;
    }

    public void OnClickAccept()
    {
        if (SetLevel.text != null && SetLevel.text != "")
        {
            indexLevelVariable.Value = int.Parse(SetLevel.text);
            prepareLevelEvent.Raise();
        }

        if (SetCoin.text != null && SetCoin.text != "")
        {
            currencyTotalVariable.Value = int.Parse(SetCoin.text);
        }

        SetCoin.text = string.Empty;
        SetLevel.text = string.Empty;
        DOTween.Sequence().AppendInterval(1).AppendCallback(() => { Hide(); });
    }

    public void ChangeTestingState()
    {
        isTestingVariable.Value = ToggleTesting.isOn;
    }

    public void OnClickFPSBtn()
    {
        // changeFpsEvent.Raise();
    }

    public void OnClickUnlockAllSkin()
    {
        Config.ItemConfig.UnlockAllSkins();
    }

    public void OnClickIsOffInterAds()
    {
        isOffInterAdsVariable.Value = ToggleIsOffInterAds.isOn;
    }
}