using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheBeginning.Config
{
    [CreateAssetMenu(fileName = "VisualEffectConfig", menuName = "Config/VisualEffectConfig")]
    public class VisualEffectConfig : ScriptableObject
    {
        [SerializeField] private List<VisualEffectData> listVisualEffectData;
        public List<VisualEffectData> ListVisualEffectData => listVisualEffectData;

        public VisualEffectData GetVisualEffectDataByType(VisualEffectType visualEffectType)
        {
            return listVisualEffectData.Find(item => item.visualEffectType == visualEffectType);
        }
    }

    [Serializable]
    public class VisualEffectData
    {
        public VisualEffectType visualEffectType;
        public List<GameObject> effectList;

        public GameObject GetRandomEffect()
        {
            return effectList[Random.Range(0, effectList.Count)];
        }
    }

    public enum VisualEffectType
    {
        Default,
    }
}