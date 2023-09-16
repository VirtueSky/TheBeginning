using TMPro;
using UnityEngine.UI;

public class PopupDebug : UIPopup
{
    public TMP_InputField SetLevel;
    public TMP_InputField SetCoin;
    public Toggle ToggleTesting;
    public Toggle ToggleIsOffInterAds;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        ToggleTesting.isOn = Data.IsTesting;
        ToggleIsOffInterAds.isOn = Data.IsOffInterAds;
    }

    public void OnClickAccept()
    {
        if (SetLevel.text != null && SetLevel.text != "")
        {
            Data.CurrentLevel = int.Parse(SetLevel.text);
            GameManager.Instance.PrepareLevel();
            GameManager.Instance.StartGame();
        }

        if (SetCoin.text != null && SetCoin.text != "")
        {
            Data.CurrencyTotal = int.Parse(SetCoin.text);
        }

        SetCoin.text = string.Empty;
        SetLevel.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void ChangeTestingState()
    {
        Data.IsTesting = ToggleTesting.isOn;
    }

    public void OnClickFPSBtn()
    {
        GameManager.Instance.ChangeAFpsState();
    }

    public void OnClickUnlockAllSkin()
    {
        Config.ItemConfig.UnlockAllSkins();
        Observer.PurchaseSucceed?.Invoke();
    }

    public void OnClickIsOffInterAds()
    {
        Data.IsOffInterAds = ToggleIsOffInterAds.isOn;
    }
}