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
        [SerializeField] private List<UIPopup> listUiPopups;

        public static List<UIPopup> ItemPopupConfigs => Instance.listUiPopups;

        public static UIPopup GetPrefabPopup(string popupName)
        {
            Debug.Log($"Count: {ItemPopupConfigs.Count}");
            return ItemPopupConfigs.FirstOrDefault(item => item.name == popupName);
        }
#if UNITY_EDITOR
        [Button]
        void LoadPrefabPopup()
        {
            listUiPopups.Clear();
            var popups = VirtueSky.UtilsEditor.FileExtension.FindAll<GameObject>(pathLoad);
            foreach (var obj in popups)
            {
                UIPopup popup = obj.GetComponent<UIPopup>();
                if (popup != null)
                {
                    listUiPopups.Add(popup);
                }
            }
        }
#endif
    }
}