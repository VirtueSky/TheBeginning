#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using Unity.Services.LevelPlay.Editor.AdapterData;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class LineItemComponent : IDrawable
    {
        readonly IntegrationManagerNormalText networkName;
        readonly IntegrationManagerVersion currentVersion;
        readonly IntegrationManagerVersion latestVersion;
        readonly IntegrationManagerButton button;
        readonly IntegrationManagerRecommendedBox recommended;
        readonly bool isRecommended;
        readonly Adapter adapter;
        const int k_networkNameFontSize = 12;

        public LineItemComponent(
            Texture2D recommendedIconPath,
            Adapter adapter,
            string networkName,
            string tooltipText,
            Action<Adapter> onNetworkButtonClick)
        {
            this.adapter = adapter;
            this.networkName = new IntegrationManagerNormalText(networkName, k_networkNameFontSize);
            currentVersion = new IntegrationManagerVersion(adapter.CurrentVersion, 75);
            latestVersion = new IntegrationManagerVersion(adapter.LatestVersion, 200);
            button = new IntegrationManagerButton(
                IntegrationManagerUIUtils.GetButtonText(adapter.CurrentStatus, string.Empty),
                tooltipText,
                () => onNetworkButtonClick.Invoke(adapter));
            recommended = new IntegrationManagerRecommendedBox(recommendedIconPath);
            isRecommended = adapter.IsRecommended;
        }

        public void Draw()
        {
            SetGuiEnablementBasedOnStatus(adapter.CurrentStatus);
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            networkName.Draw();

            if (isRecommended)
            {
                recommended.Draw();
            }

            GUILayout.FlexibleSpace();
            currentVersion.Draw();
            latestVersion.Draw();
            button.Draw();
            GUILayout.Space(4);
            GUILayout.EndHorizontal();
        }

        static void SetGuiEnablementBasedOnStatus(Adapter.Status status)
        {
            switch (status)
            {
                case Adapter.Status.NotInstalled:
                    GUI.enabled = true;
                    break;
                case Adapter.Status.UpdateAvailable:
                    GUI.enabled = true;
                    break;
                case Adapter.Status.UpToDate:
                default:
                    GUI.enabled = false;
                    break;
            }
        }
    }
}
#endif
