using System;
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "ScriptableObject/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public List<SoundData> SoundDatas;

#if UNITY_EDITOR
    [Button]
    public void UpdateSoundDatas()
    {
        for (int i = 0; i < Enum.GetNames(typeof(SoundType)).Length; i++)
        {
            SoundData soundData = new SoundData();
            soundData.SoundType = (SoundType)i;
            if (IsItemExistedBySoundType(soundData.SoundType)) continue;
            SoundDatas.Add(soundData);
        }

        SoundDatas = SoundDatas.GroupBy(elem => elem.SoundType).Select(group => group.First()).ToList();
    }
#endif

    private bool IsItemExistedBySoundType(SoundType soundType)
    {
        foreach (SoundData item in SoundDatas)
        {
            if (item.SoundType == soundType)
            {
                return true;
            }
        }

        return false;
    }

    public SoundData GetSoundDataByType(SoundType soundType)
    {
        return SoundDatas.Find(item => item.SoundType == soundType);
    }
}

[Serializable]
public class SoundData
{
    public SoundType SoundType;
    public List<AudioClip> Clips;

    public AudioClip GetRandomAudioClip()
    {
        if (Clips.Count > 0)
        {
            return Clips[Random.Range(0, Clips.Count)];
        }

        return null;
    }
}

public enum SoundType
{
    BackgroundInGame,
    BackgroundHome,
    ClickButton,
    PurchaseFail,
    PurchaseSucceed,
    ClaimReward,
    StartLevel,
    WinLevel,
    LoseLevel,
    ShowPopupWin,
    ShowPopupLose,
    CoinMove,
}