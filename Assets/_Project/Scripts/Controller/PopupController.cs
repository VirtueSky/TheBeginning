using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public Transform canvasTransform;
    public CanvasScaler canvasScaler;
    public Camera cameraUI;
    public List<Popup> popups;
    private readonly Dictionary<Type, Popup> _dictionary = new Dictionary<Type, Popup>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Initialize();
        Debug.Assert(cameraUI != null, "Camera.main != null");
        canvasScaler.matchWidthOrHeight = cameraUI.aspect > .7f ? 1 : 0;
    }

    public void Initialize()
    {
        int index = 0;
        popups.ForEach(popup =>
        {
            Popup popupInstance = Instantiate(popup, canvasTransform);
            popupInstance.gameObject.SetActive(false);
            popupInstance.Canvas.sortingOrder = index++;
            _dictionary.Add(popupInstance.GetType(), popupInstance);
        });
    }

    public void Show(Type T)
    {
        if (_dictionary.TryGetValue(T, out Popup popup))
        {
            if (!popup.isActiveAndEnabled)
            {
                popup.Show();
            }
        }
    }

    public void Hide(Type T)
    {
        if (_dictionary.TryGetValue(T, out Popup popup))
        {
            if (popup.isActiveAndEnabled)
            {
                popup.Hide();
            }
        }
    }

    public void HideAll()
    {
        foreach (Popup item in _dictionary.Values)
        {
            if (item.isActiveAndEnabled)
            {
                item.Hide();
            }
        }
    }

    public Popup Get(Type T)
    {
        if (_dictionary.TryGetValue(T, out Popup popup))
        {
            return popup;
        }

        return null;
    }
}