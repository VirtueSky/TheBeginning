using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

public class OffUI : MonoBehaviour
{
    [SerializeField] private BooleanVariable debugOnOffUIVariable;
    [SerializeField] private List<Graphic> listGraphics = new List<Graphic>();

    private void OnEnable()
    {
        GetComponentUI();
        debugOnOffUIVariable.AddListener(Setup);
        Setup(debugOnOffUIVariable.Value);
    }

    private void OnDisable()
    {
        debugOnOffUIVariable.RemoveListener(Setup);
    }

    void Setup(bool isOff)
    {
        if (listGraphics.Count == 0) return;
        foreach (var graphic in listGraphics)
        {
            graphic.SetAlpha(isOff ? 1 : 0);
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