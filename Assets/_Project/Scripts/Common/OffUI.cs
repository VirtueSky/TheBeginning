using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class OffUI : MonoBehaviour
{
    [SerializeField] private BooleanVariable isOffUIVariable;
    [SerializeField] private List<Graphic> listGraphics;

    private void OnEnable()
    {
        isOffUIVariable.AddListener(Setup);
        Setup(isOffUIVariable.Value);
    }

    private void OnDisable()
    {
        isOffUIVariable.RemoveListener(Setup);
    }

    void Setup(bool isOff)
    {
        if (listGraphics.Count == 0) return;
        foreach (var graphic in listGraphics)
        {
            graphic.DOFade(isOff ? 0 : 1, 0);
        }
    }
#if UNITY_EDITOR
    [Button]
    void GetComponentUI()
    {
        listGraphics = GetComponentsInChildren<Graphic>(true).ToList();
    }

    private void Reset()
    {
        GetComponentUI();
    }
#endif
}