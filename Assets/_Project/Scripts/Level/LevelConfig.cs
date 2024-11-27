using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace TheBeginning.LevelSystem
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class LevelConfig : ScriptableSettings<LevelConfig>
    {
        [SerializeField] private int maxLevel;
        [SerializeField] private int startLoopLevel;
        [SerializeField] private string pathLoad = "Assets/_Project/Prefabs/Levels";
        [TableList, SerializeField] private List<ItemLevelConfig> itemLevelConfigs;


        public static int MaxLevel => Instance.maxLevel;
        public static int StartLoopLevel => Instance.startLoopLevel;
        public static List<ItemLevelConfig> ItemLevelConfigs => Instance.itemLevelConfigs;

        public int HandleIndexLevel(int indexLevel)
        {
            if (indexLevel > maxLevel)
            {
                return (indexLevel - startLoopLevel) %
                       (maxLevel - startLoopLevel + 1) +
                       startLoopLevel;
            }

            if (indexLevel > 0 && indexLevel <= maxLevel)
            {
                return indexLevel;
            }

            if (indexLevel == 0)
            {
                return maxLevel;
            }

            return 1;
        }


#if UNITY_EDITOR
        [Button]
        void LoadPrefabLevel()
        {
            itemLevelConfigs.Clear();
            List<GameObject> levels = VirtueSky.UtilsEditor.FileExtension.FindAll<GameObject>(pathLoad);
            for (var i = 0; i < levels.Count; i++)
            {
                itemLevelConfigs.Add(new ItemLevelConfig(levels[i].GetComponent<Level>()));
            }
        }
#endif
    }

    [Serializable]
    public class ItemLevelConfig
    {
        [ReadOnly] public string key;
        public Level levelPrefab;

        public ItemLevelConfig(Level _levelPrefab)
        {
            key = _levelPrefab.name;
            this.levelPrefab = _levelPrefab;
        }
    }
}