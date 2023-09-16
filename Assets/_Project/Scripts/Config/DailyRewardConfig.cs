using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "ScriptableObject/DailyRewardConfig")]
public class DailyRewardConfig : ScriptableObject
{
    public List<DailyRewardData> DailyRewardDatas;
    public List<DailyRewardData> DailyRewardDatasLoop;
}

[Serializable]
public class DailyRewardData
{
    public DailyRewardType DailyRewardType;
    public Sprite Icon;

    [ShowIf("DailyRewardType", DailyRewardType.Skin)]
    public string SkinID;

    [ShowIf("DailyRewardType", DailyRewardType.Currency)]
    public int Value;
}

public enum DailyRewardType
{
    Currency,
    Skin,
}