using TMPro;
using UnityEngine;
using VirtueSky.Variables;

public class PopupUpdate : UIPopup
{
    [Space] [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private TextMeshProUGUI textVersion;
    [SerializeField] private StringVariable contentUpdateVariable;
    [SerializeField] private StringVariable versionUpdateVariable;

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
        Setup();
    }

    void Setup()
    {
        textContent.text = contentUpdateVariable.Value;
        textVersion.text = versionUpdateVariable.Value;
    }
}