using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Linq;

namespace TheBeginning.LevelSystem
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private int maxLevel;
        [SerializeField] private int startLoopLevel;

        [SerializeField] private string pathLoad = "Assets/_Project/Prefabs/Levels";
        [SerializeField] private List<Level> listLevels;


        public int MaxLevel => maxLevel;
        public int StartLoopLevel => startLoopLevel;
        public List<Level> ListLevels => listLevels;

        public Level GePrefabLevel(string levelName)
        {
            return listLevels.FirstOrDefault(item => item.name == levelName);
        }

#if UNITY_EDITOR
        [Button]
        void LoadPrefabLevel()
        {
            listLevels.Clear();
            List<GameObject> levels = VirtueSky.UtilsEditor.FileExtension.FindAll<GameObject>(pathLoad);
            for (var i = 0; i < levels.Count; i++)
            {
                if (levels[i].GetComponent<Level>() != null)
                {
                    listLevels.Add(levels[i].GetComponent<Level>());
                }
            }
        }

#endif
    }
}