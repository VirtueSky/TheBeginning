using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace TheBeginning.UI
{
    [EditorIcon("icon_generator")]
    public class PopupManager : BaseMono
    {
        [HeaderLine(Constant.Environment)] [SerializeField]
        private Transform parentContainer;

        [SerializeField] private Camera cameraUI;

        [HeaderLine(Constant.SO_Variable)] [SerializeField]
        private readonly Dictionary<Type, UIPopup> container = new Dictionary<Type, UIPopup>();

        private int index = 1;

        private static PopupManager _ins;

        private void Awake()
        {
            Debug.Assert(cameraUI != null, "CameraUI != null");
            if (_ins == null)
            {
                _ins = this;
            }
        }

        private async void InternalShow<T>(bool isHideAll = true, Action showPopupCompleted = null)
        {
            container.TryGetValue(typeof(T), out UIPopup popup);
            if (popup == null)
            {
                var obj = await Addressables.LoadAssetAsync<GameObject>(GetKeyPopup(typeof(T).ToString()));
                var popupPrefab = obj.GetComponent<UIPopup>();
                if (popupPrefab != null)
                {
                    var popupInstance = Instantiate(popupPrefab, parentContainer);
                    if (isHideAll)
                    {
                        InternalHideAll();
                    }

                    popupInstance.Show();
                    showPopupCompleted?.Invoke();
                    container.Add(popupInstance.GetType(), popupInstance);
                    popupInstance.canvas.sortingOrder = index++;
                }
                else
                {
                    Debug.Log("Popup not found in the list to show");
                }
            }
            else
            {
                if (!popup.isActiveAndEnabled)
                {
                    if (isHideAll)
                    {
                        InternalHideAll();
                    }

                    popup.Show();
                    showPopupCompleted?.Invoke();
                }
            }
        }

        private void InternalHide<T>(Action hidePopupCompleted = null)
        {
            if (container.TryGetValue(typeof(T), out UIPopup popup))
            {
                if (popup.isActiveAndEnabled)
                {
                    popup.Hide();
                    hidePopupCompleted?.Invoke();
                }
            }
            else
            {
                Debug.Log("Popup not found to hide");
            }
        }

        private UIPopup InternalGet<T>()
        {
            return container.GetValueOrDefault(typeof(T));
        }

        private bool InternalIsPopupReady<T>()
        {
            return container.ContainsKey(typeof(T));
        }

        public void InternalHideAll()
        {
            foreach (var popup in container.Values)
            {
                if (popup.isActiveAndEnabled)
                {
                    popup.Hide();
                }
            }
        }

        string GetKeyPopup(string fullName)
        {
            int index = fullName.LastIndexOf('.');
            if (index != -1)
            {
                return fullName.Substring(index + 1).Trim();
            }
            else
            {
                return fullName;
            }
        }

        #region API

        public static void Show<T>(bool isHideAll = true, Action showPopupCompleted = null) => _ins.InternalShow<T>(isHideAll, showPopupCompleted);
        public static void Hide<T>(Action hidePopupCompleted = null) => _ins.InternalHide<T>(hidePopupCompleted);
        public static UIPopup Get<T>() => _ins.InternalGet<T>();
        public static bool IsPopupReady<T>() => _ins.InternalIsPopupReady<T>();
        public static void HideAll() => _ins.InternalHideAll();

        #endregion
    }
}