using UnityEngine;

namespace TheBeginning.AppControl
{
    public struct AppControlPopup
    {
        private static PopupManager _popupManager;

        public static void Init(PopupManager popupManager)
        {
            if (_popupManager != null)
            {
                UnityEngine.Object.Destroy(_popupManager);
            }

            AppControlPopup._popupManager = popupManager;
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
    }
}