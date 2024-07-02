#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.LevelPlay.Editor.AdapterData;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class NetworkBoxComponentIS : IDrawable
    {
        readonly IntegrationManagerNormalText _networkName;
        readonly ICollection<LineItemComponent> _lineItemComponents;
        const string k_MediationSdkName = "LevelPlay Mediation";
        const string k_NetworkSdkName = "ironSource Ads";
        const int k_networkNameFontSize = 12;

        public NetworkBoxComponentIS(
            ICollection<Adapter> networkData,
            Action<Adapter> onNetworkButtonClick)
        {
            _networkName = new IntegrationManagerNormalText(k_NetworkSdkName, k_networkNameFontSize);
            _lineItemComponents = networkData.Select(adapter => new LineItemComponent(
                null,
                adapter,
                k_MediationSdkName,
                IntegrationManagerUIUtils.GetToolTipText(adapter),
                onNetworkButtonClick)).ToList();
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.ExpandHeight(false)))
            {
                foreach (var lineItem in _lineItemComponents)
                {
                    lineItem.Draw();
                    GUILayout.Space(4);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(4);
                    _networkName.Draw();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUI.enabled = true;
                }
            }
        }
    }
}
#endif
