using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Variables;

public class PopupAdministrator : UIPopup
{
    [SerializeField] private Toggle toggleOffUI;
    [SerializeField] private BooleanVariable isOffUIVariable;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        SetupDefault();
    }

    void SetupDefault()
    {
        toggleOffUI.isOn = isOffUIVariable.Value;
    }

    void Setup()
    {
        isOffUIVariable.Value = toggleOffUI.isOn;
    }

    public void Exit()
    {
        Setup();
        Hide();
    }
}