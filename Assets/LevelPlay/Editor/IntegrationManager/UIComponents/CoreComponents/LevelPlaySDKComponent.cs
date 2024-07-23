#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using Unity.Services.LevelPlay.Editor.AdapterData;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class LevelPlaySDKComponent : IDrawable
    {
        readonly IntegrationManagerCoreHeader header;
        readonly IntegrationManagerNormalText normalText;
        readonly NetworkBoxComponentIS networkBox;
        const int k_infoTextFontSize = 11;

        public LevelPlaySDKComponent(
            ICollection<Adapter> networkData,
            Action<Adapter> onNetworkButtonClick)
        {
            normalText = new IntegrationManagerNormalText("A bundled SDK that includes LevelPlay Mediation " +
                                                          "and the ironSource Ad Network.", k_infoTextFontSize);
            header = new IntegrationManagerCoreHeader("ironSource SDK");
            networkBox = new NetworkBoxComponentIS(networkData, onNetworkButtonClick);
        }

        public void Draw()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Space(10);

                GUILayout.BeginVertical();
                GUILayout.Space(10);
                header.Draw();
                GUILayout.Space(5);
                normalText.Draw();
                GUILayout.Space(5);
                networkBox.Draw();
                GUILayout.EndVertical();

                GUILayout.Space(10);
                GUI.enabled = true;
            }
        }
    }
}
#endif
