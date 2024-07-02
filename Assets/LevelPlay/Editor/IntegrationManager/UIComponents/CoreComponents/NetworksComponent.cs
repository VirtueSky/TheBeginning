#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using Unity.Services.LevelPlay.Editor.AdapterData;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class NetworksComponent: IDrawable
    {
        readonly IntegrationManagerCoreHeader header;
        readonly NetworkBoxComponent networksBox;

        public NetworksComponent(
            Texture2D recommendedIcon,
            ICollection<Adapter> networkData,
            Action<Adapter> onNetworkButtonClick)
        {
            header = new IntegrationManagerCoreHeader("Networks");
            networksBox = new NetworkBoxComponent(recommendedIcon, networkData, onNetworkButtonClick);
        }

        public void Draw()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
            {
                GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    header.Draw();
                    networksBox.Draw();
                    GUILayout.EndVertical();
                GUILayout.Space(10);
            }
        }
    }
}
#endif
