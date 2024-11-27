using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Linq;
using VirtueSky.Utils;

namespace TheBeginning.UI
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class PopupConfig : ScriptableSettings<PopupConfig>
    {
        [SerializeField] private string pathLoad = "Assets/_Project/Prefabs/Popups";
        [TableList, SerializeField] private List<ItemPopupConfig> itemPopupConfigs;

        public static List<ItemPopupConfig> ItemPopupConfigs => Instance.itemPopupConfigs;

        public static UIPopup GetPrefabPopup(string key)
        {
            return ItemPopupConfigs.FirstOrDefault(item => item.key == key).popupPrefab;
        }
#if UNITY_EDITOR
        [Button]
        void LoadPrefabPopup()
        {
            itemPopupConfigs.Clear();
            var popups = VirtueSky.UtilsEditor.FileExtension.FindAll<GameObject>(pathLoad);
            foreach (var obj in popups)
            {
                UIPopup popup = obj.GetComponent<UIPopup>();
                if (popup != null)
                {
                    itemPopupConfigs.Add(new ItemPopupConfig(popup));
                }
            }
        }

        private void OnValidate()
        {
            foreach (var itemPopupConfig in itemPopupConfigs)
            {
                itemPopupConfig.SetupKey();
            }
        }
#endif
    }

    [Serializable]
    public class ItemPopupConfig
    {
        [ReadOnly] public string key;
        public UIPopup popupPrefab;

        public ItemPopupConfig(UIPopup _popup)
        {
            popupPrefab = _popup;
            SetupKey();
        }

        public void SetupKey()
        {
            key = popupPrefab.name;
        }
    }
}