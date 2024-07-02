#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.LevelPlay.Editor.AdapterData;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class NetworkBoxComponent: IDrawable
    {
        readonly ICollection<LineItemComponent> _lineItemComponents;

        public NetworkBoxComponent(
            Texture2D recommendedIcon,
            ICollection<Adapter> networkData,
            Action<Adapter> onNetworkButtonClick)
        {
            _lineItemComponents = networkData.Select(adapter => new LineItemComponent(
                recommendedIcon,
                adapter,
                GetNetworkName(adapter),
                IntegrationManagerUIUtils.GetToolTipText(adapter),
                onNetworkButtonClick)).ToList();
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope("box", GUILayout.ExpandHeight(true)))
            {
                GUILayout.Space(4);
                foreach (var lineItem in _lineItemComponents)
                {
                    lineItem.Draw();
                    GUI.enabled = true;
                    GUILayout.Space(6);
                }
            }
        }

        static string GetNetworkName(Adapter adapter)
        {
            var networkName = adapter.AdapterName;
            if (adapter.IsNewAdapter)
            {
                networkName += " - New Network";
            }

            return networkName;
        }
    }
}
#endif
