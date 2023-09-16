using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Newtonsoft.Json;
using UnityEngine;

public static class Utility
{
    public static void Clear(this Transform transform)
    {
        var childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject, true);
        }
    }

    public static float GetScreenRatio()
    {
        return (1920f / 1080f) / (Screen.height / (float)Screen.width);
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }

        return null;
    }

    public static int GetNumberInAString(string str)
    {
        try
        {
            var getNumb = Regex.Match(str, @"\d+").Value;
            return Int32.Parse(getNumb);
        }
        catch (Exception e)
        {
            return -1;
        }

        return -1;
    }

    public static void SaveDataByJson<T>(string _key, T _saveData)
    {
        string data = JsonUtility.ToJson(_saveData);
        PlayerPrefs.SetString(_key, data);
    }

    public static T GetDataByJson<T>(string _key)
    {
        string json = PlayerPrefs.GetString(_key);
        //  if (!string.IsNullOrEmpty(json))
        //  {
        T getData = JsonConvert.DeserializeObject<T>(json);
        return getData;
        //  }
    }

    public static void SetLayerForAllChild(GameObject obj, int layerIndex)
    {
        obj.layer = layerIndex;
        obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.layer = layerIndex; });
    }

    public static void SetLayerForAllChild(GameObject obj, string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);
        obj.layer = layerIndex;
        obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.layer = layerIndex; });
    }

    public static void DelayByTweenAppendInterval(float timeDelay, Action callBack, Action completed = null)
    {
        DOTween.Sequence().AppendInterval(timeDelay).AppendCallback(() => { callBack?.Invoke(); }).OnComplete(() => { completed?.Invoke(); });
    }

    static bool isVibrate = true;

    public static void HapticLimit(HapticTypes _type)
    {
        if (isVibrate)
        {
            isVibrate = false;
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            DelayByTweenAppendInterval(4, () => isVibrate = true);
        }
    }
}