using System.Collections.Generic;
using UnityEngine;
using System;
using TheBeginning.AppControl;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;

[EditorIcon("icon_generator")]
public class PopupManager : BaseMono
{
    [HeaderLine(Constant.Environment)] [SerializeField]
    private Transform parentContainer;

    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Camera cameraUI;

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private readonly Dictionary<Type, UIPopup> _container = new Dictionary<Type, UIPopup>();

    private int index = 1;

    private void Awake()
    {
        Debug.Assert(cameraUI != null, "CameraUI != null");
        canvasScaler.matchWidthOrHeight = cameraUI.aspect > .6f ? 1 : 0;
        AppControlPopup.Init(this);
    }

    public async void Show<T>(bool isHideAll = true)
    {
        _container.TryGetValue(typeof(T), out UIPopup popup);
        if (popup == null)
        {
            var obj = await Addressables.LoadAssetAsync<GameObject>(GetKeyPopup(typeof(T).ToString()));
            var popupPrefab = obj.GetComponent<UIPopup>();
            if (popupPrefab != null)
            {
                var popupInstance = Instantiate(popupPrefab, parentContainer);
                if (isHideAll)
                {
                    HideAll();
                }

                popupInstance.Show();
                _container.Add(popupInstance.GetType(), popupInstance);
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
                    HideAll();
                }

                popup.Show();
            }
        }
    }

    public void Hide<T>()
    {
        if (_container.TryGetValue(typeof(T), out UIPopup popup))
        {
            if (popup.isActiveAndEnabled)
            {
                popup.Hide();
            }
        }
        else
        {
            Debug.Log("Popup not found to hide");
        }
    }

    public UIPopup Get<T>()
    {
        if (_container.TryGetValue(typeof(T), out UIPopup popup))
        {
            return popup;
        }

        return null;
    }

    public bool IsPopupReady<T>()
    {
        return _container.ContainsKey(typeof(T));
    }

    public void HideAll()
    {
        foreach (var popup in _container.Values)
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
}