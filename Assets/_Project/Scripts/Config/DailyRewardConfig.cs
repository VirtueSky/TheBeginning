using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

[CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "Config/DailyRewardConfig")]
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