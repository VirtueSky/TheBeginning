using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Variables;

namespace TheBeginning.UI
{
    public class UpdatePopup : UIPopup
    {
        [Space] [SerializeField] private TextMeshProUGUI textContent;
        [SerializeField] private TextMeshProUGUI textVersion;
        [SerializeField] private StringVariable contentUpdateVariable;
        [SerializeField] private StringVariable versionUpdateVariable;
        [SerializeField] private BooleanVariable dontShowAgainPopupUpdate;
        [SerializeField] private Toggle toggleShowAgain;

        private void Start()
        {
            toggleShowAgain.isOn = false;
        }

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

        public void OnChangeValueShowAgain()
        {
            dontShowAgainPopupUpdate.Value = toggleShowAgain.isOn;
        }
    }
}