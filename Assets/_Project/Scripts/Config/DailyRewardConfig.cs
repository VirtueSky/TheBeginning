using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using VirtueSky.Inspector;

[CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "Config/DailyRewardConfig")]
public class DailyRewardConfig : ScriptableObject
{
    [SerializeField] private List<DailyRewardData> dailyRewardDatas;
    [SerializeField] private List<DailyRewardData> dailyRewardDatasLoop;
#if UNITY_EDITOR
    [Space(20)] [SerializeField] bool enableSetData;

    [ShowIf(nameof(enableSetData))] [SerializeField]
    private Sprite iconData;

    [ShowIf(nameof(enableSetData))] [SerializeField]
    private int totalData;

    [ShowIf(nameof(enableSetData))] [SerializeField]
    private int coinValueDefault;

    [ShowIf(nameof(enableSetData)), Button]
    void SetDataDefault()
    {
        dailyRewardDatas.Clear();
        for (int i = 0; i < totalData; i++)
        {
            DailyRewardData dailyRewardData = new DailyRewardData(iconData, coinValueDefault);
            dailyRewardDatas.Add(dailyRewardData);
        }

        EditorUtility.SetDirty(this);
    }

    [Space(20)] [SerializeField] bool enableSetDataLoop;

    [ShowIf(nameof(enableSetDataLoop))] [SerializeField]
    private Sprite iconDataLoop;

    [ShowIf(nameof(enableSetDataLoop))] [SerializeField]
    private int totalDataLoop;

    [ShowIf(nameof(enableSetDataLoop))] [SerializeField]
    private int coinValueDefaultLoop;

    [ShowIf(nameof(enableSetDataLoop)), Button]
    void SetDataDefaultLoop()
    {
        dailyRewardDatasLoop.Clear();
        for (int i = 0; i < totalDataLoop; i++)
        {
            DailyRewardData dailyRewardData = new DailyRewardData(iconDataLoop, coinValueDefaultLoop);
            dailyRewardDatasLoop.Add(dailyRewardData);
        }

        EditorUtility.SetDirty(this);
    }
#endif


    public List<DailyRewardData> DailyRewardDatas => dailyRewardDatas;
    public List<DailyRewardData> DailyRewardDatasLoop => dailyRewardDatasLoop;
}

[Serializable]
public class DailyRewardData
{
    public DailyRewardType dailyRewardType;
    public Sprite icon;

    [ShowIf(nameof(dailyRewardType), DailyRewardType.Skin)]
    public string skinID;

    [ShowIf(nameof(dailyRewardType), DailyRewardType.Currency)]
    public int value;

    public DailyRewardData(Sprite _icon, int _value)
    {
        this.dailyRewardType = DailyRewardType.Currency;
        this.icon = _icon;
        this.value = _value;
        this.skinID = "";
    }
}

public enum DailyRewardType
{
    Currency,
    Skin,
}