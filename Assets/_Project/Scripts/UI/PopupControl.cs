using UnityEngine;

namespace TheBeginning.AppControl
{
    public struct PopupControl
    {
        private static PopupManager _popupManager;

        public static bool IsPopupManagerReady()
        {
            return _popupManager != null;
        }

        public static void Init(PopupManager popupManager)
        {
            if (_popupManager != null)
            {
                UnityEngine.Object.Destroy(_popupManager);
            }

            PopupControl._popupManager = popupManager;
        }

        public static void Show<T>(bool isHideAll = true)
        {
            if (_popupManager == null)
            {
                Debug.LogError("Please Init AppControlPopup before use");
                return;
            }

            _popupManager.Show<T>(isHideAll);
        }

        public static void Hide<T>()
        {
            if (_popupManager == null)
            {
                Debug.LogError("Please Init AppControlPopup before use");
                return;
            }

            _popupManager.Hide<T>();
        }

        public static void HideAll()
        {
            if (_popupManager == null)
            {
                Debug.LogError("Please Init AppControlPopup before use");
                return;
            }

            _popupManager.HideAll();
        }

        public static void Get<T>()
        {
            if (_popupManager == null)
            {
                Debug.LogError("Please Init AppControlPopup before use");
                return;
            }

            _popupManager.Get<T>();
        }

        public static bool IsPopupReady<T>()
        {
            if (_popupManager == null)
            {
                Debug.LogError("Please Init AppControlPopup before use");
                return false;
            }

            return _popupManager.IsPopupReady<T>();
        }
    }
}