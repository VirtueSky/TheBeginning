using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Linq;
using VirtueSky.Utils;
using VirtueSky.Variables;

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

        public static Level GePrefabLevel(string key)
        {
            return ItemLevelConfigs.FirstOrDefault(item => item.key == key).levelPrefab;
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

        private void OnValidate()
        {
            for (var i = 0; i < itemLevelConfigs.Count; i++)
            {
                itemLevelConfigs[i].SetupKey();
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
            this.levelPrefab = _levelPrefab;
            SetupKey();
        }

        public void SetupKey()
        {
            key = levelPrefab.name;
        }
    }
}