using System.Collections.Generic;
using UnityEngine;
using System;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Linq;
using VirtueSky.Misc;
using VirtueSky.Utils;

namespace TheBeginning.UI
{
    [EditorIcon("icon_generator")]
    public class PopupManager : BaseMono
    {
        [HeaderLine(Constant.Environment, false, CustomColor.Gold, CustomColor.Aqua)] [SerializeField]
        private Transform parentContainer;

        [SerializeField] private Camera cameraUI;

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

        private void InternalShow<T>(bool isHideAll = true, Action showPopupCompleted = null)
        {
            container.TryGetValue(typeof(T), out UIPopup popup);
            if (popup == null)
            {
                var popupPrefab = PopupConfig.GetPrefabPopup(typeof(T).Name);
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
                    Debug.Log("Popup not found in the list to show".SetColor(Color.red));
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
                Debug.Log("Popup not found to hide".SetColor(Color.red));
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

        private void InternalHideAll()
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

        public static void Show<T>(bool isHideAll = true, Action showPopupCompleted = null) =>
            _ins.InternalShow<T>(isHideAll, showPopupCompleted);

        public static void Hide<T>(Action hidePopupCompleted = null) => _ins.InternalHide<T>(hidePopupCompleted);
        public static UIPopup Get<T>() => _ins.InternalGet<T>();
        public static bool IsPopupReady<T>() => _ins.InternalIsPopupReady<T>();
        public static void HideAll() => _ins.InternalHideAll();

        #endregion
    }
}