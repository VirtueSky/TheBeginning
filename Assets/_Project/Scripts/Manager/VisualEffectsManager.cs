using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtueSky.Attributes;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;
using Random = UnityEngine.Random;


public class VisualEffectsManager : BaseMono
{
    public List<VisualEffectData> visualEffectDatas;
    [SerializeField] private Pools pools;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        pools.Initialize();
    }

    public VisualEffectData GetVisualEffectDataByType(VisualEffectType visualEffectType)
    {
        return visualEffectDatas.Find(item => item.visualEffectType == visualEffectType);
    }

    public void SpawnEffect(SpawnEffectData data)
    {
        VisualEffectData visualEffectData = GetVisualEffectDataByType(data.visualEffectType);
        if (visualEffectData != null)
        {
            GameObject randomEffect = visualEffectData.GetRandomEffect();
            GameObject effect = pools.Spawn(randomEffect, data.parent, false);
            effect.transform.position = data.position;
            effect.transform.localScale = (data.localScale == default) ? Vector3.one : data.localScale;

            if (data.isDestroyedOnEnd) Destroy(effect, data.timeDestroy);
        }
    }

    private bool IsItemExistedByVisualEffectType(VisualEffectType visualEffectType)
    {
        foreach (VisualEffectData item in visualEffectDatas)
        {
            if (item.visualEffectType == visualEffectType)
            {
                return true;
            }
        }

        return false;
    }

    [Button]
    public void UpdateVisualEffects()
    {
        for (int i = 0; i < Enum.GetNames(typeof(VisualEffectType)).Length; i++)
        {
            VisualEffectData visualEffectData = new VisualEffectData();
            visualEffectData.visualEffectType = (VisualEffectType)i;
            if (IsItemExistedByVisualEffectType(visualEffectData.visualEffectType)) continue;
            visualEffectDatas.Add(visualEffectData);
        }

        visualEffectDatas = visualEffectDatas.GroupBy(elem => elem.visualEffectType).Select(group => group.First())
            .ToList();
    }
}

[Serializable]
public class VisualEffectData
{
    public List<GameObject> effectList;
    public VisualEffectType visualEffectType;

    public GameObject GetRandomEffect()
    {
        return effectList[Random.Range(0, effectList.Count)];
    }
}

public enum VisualEffectType
{
    Default,
}