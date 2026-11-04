using TheBeginning.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VirtueSky.Events;

namespace TheBeginning.UI
{
    public class DebugViewUnlocker : MonoBehaviour, IPointerClickHandler
    {
        private const string Password = "zbyte12345";

        [SerializeField] private int requiredTapCount = 10;
        [SerializeField] private float maxTapInterval = 0.35f;

        [Header("Password Panel")] [SerializeField]
        private GameObject passwordPanel;

        [SerializeField] private TMP_InputField inputPassword;
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttonClose;
        [SerializeField] private TextMeshProUGUI textMessage;

        [Header("Notification")] [SerializeField]
        private StringEvent showNotificationInGameEvent;

        private int tapCount;
        private float lastTapTime;

        private void OnEnable()
        {
            ResetTap();
            HidePanel();
            if (buttonConfirm != null) buttonConfirm.onClick.AddListener(OnClickConfirm);
            if (buttonClose != null) buttonClose.onClick.AddListener(HidePanel);
            if (inputPassword != null) inputPassword.onSubmit.AddListener(OnSubmitPassword);
        }

        private void OnDisable()
        {
            if (buttonConfirm != null) buttonConfirm.onClick.RemoveListener(OnClickConfirm);
            if (buttonClose != null) buttonClose.onClick.RemoveListener(HidePanel);
            if (inputPassword != null) inputPassword.onSubmit.RemoveListener(OnSubmitPassword);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (UserData.EnableDebugView) return;
            if (passwordPanel != null && passwordPanel.activeSelf) return;

            float now = Time.unscaledTime;
            tapCount = now - lastTapTime <= maxTapInterval ? tapCount + 1 : 1;
            lastTapTime = now;

            if (tapCount < requiredTapCount) return;
            ResetTap();
            ShowPanel();
        }

        void ShowPanel()
        {
            if (passwordPanel == null) return;
            passwordPanel.SetActive(true);
            SetMessage(string.Empty);
            if (inputPassword != null)
            {
                inputPassword.text = string.Empty;
                inputPassword.ActivateInputField();
            }
        }

        void HidePanel()
        {
            if (passwordPanel != null) passwordPanel.SetActive(false);
        }

        void OnSubmitPassword(string _) => OnClickConfirm();

        void OnClickConfirm()
        {
            if (inputPassword == null) return;
            if (inputPassword.text == Password)
            {
                UserData.EnableDebugView = true;
                HidePanel();
                ShowNotification("Debug view enabled");
            }
            else
            {
                inputPassword.text = string.Empty;
                SetMessage("Wrong password");
                ShowNotification("Wrong password");
            }
        }

        void ShowNotification(string message)
        {
            if (showNotificationInGameEvent != null) showNotificationInGameEvent.Raise(message);
        }

        void SetMessage(string message)
        {
            if (textMessage != null) textMessage.text = message;
        }

        void ResetTap()
        {
            tapCount = 0;
            lastTapTime = float.NegativeInfinity;
        }
    }
}
