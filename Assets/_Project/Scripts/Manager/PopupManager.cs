using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

public class PopupManager : MonoBehaviour
{
    [HeaderLine(Constant.Environment)] [SerializeField]
    private Transform parentContainer;

    [SerializeField] private CanvasScaler canvasScaler;

    [SerializeField] private Camera cameraUI;

    [SerializeField] private List<UIPopup> listPopups = new List<UIPopup>();

    [HeaderLine(Constant.SO_Variable)] [SerializeField]
    private PopupVariable popupVariable;

    private readonly Dictionary<Type, UIPopup> _container = new Dictionary<Type, UIPopup>();
    private int index = 1;

    private void Awake()
    {
        Debug.Assert(cameraUI != null, "CameraUI != null");
        canvasScaler.matchWidthOrHeight = cameraUI.aspect > .7f ? 1 : 0;
    }

    private void Start()
    {
        popupVariable.Value = this;
    }

    public void Show<T>(bool isHideAll = true)
    {
        _container.TryGetValue(typeof(T), out UIPopup popup);
        if (popup == null)
        {
            var popupPrefab = GetPopupPrefab(typeof(T));
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
                HideAll();
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

    UIPopup GetPopupPrefab(Type T)
    {
        foreach (var popup in listPopups)
        {
            if (popup.GetType() == T)
            {
                return popup;
            }
        }

        return null;
    }
#if UNITY_EDITOR
    [SerializeField] private string pathPopup = "Assets/_Project/Prefabs/Popup/";
    [Button]
    void LoadPopup()
    {
        listPopups = FileExtension.GetPrefabsFromFolder<UIPopup>(pathPopup);
        EditorUtility.SetDirty(this);
    }
#endif
}