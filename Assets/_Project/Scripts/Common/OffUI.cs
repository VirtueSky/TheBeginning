using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class OffUI : MonoBehaviour
{
    [SerializeField] private BooleanVariable isOffUIVariable;
    [SerializeField] private List<Graphic> listGraphics;

    private void OnEnable()
    {
        GetComponentUI();
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
            graphic.SetAlpha(isOff ? 0 : 1);
        }
    }

    [Button]
    void GetComponentUI()
    {
        List<Graphic> listTemp = GetComponentsInChildren<Graphic>(true).ToList();
        listGraphics.Clear();
        foreach (var graphic in listTemp)
        {
            if (graphic.color.a != 0)
            {
                listGraphics.Add(graphic);
            }
        }
    }
#if UNITY_EDITOR
    private void Reset()
    {
        GetComponentUI();
    }
#endif
}