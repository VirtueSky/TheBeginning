#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class UnityPluginComponent : IDrawable
    {
        IntegrationManagerMajorHeaderText majorHeaderText;
        IntegrationManagerButton button;

        readonly IntegrationManagerVersion version;
        readonly IntegrationManagerNormalText normalText;
        readonly AdapterData.Adapter.Status status;

        readonly string currentlyInstalledPackageVersion;
        readonly string latestPackageVersion;
        readonly Color dividerDarkColor;
        readonly Color dividerLightColor;
        const int k_updateTextFontSize = 11;

        public UnityPluginComponent(
            (string headerTitle, Action DownloadAction) packageComponentAction,
            string currentlyInstalledPackageVersion,
            string latestPackageVersion)
        {
            this.currentlyInstalledPackageVersion = currentlyInstalledPackageVersion;
            this.latestPackageVersion = latestPackageVersion;
            majorHeaderText = new IntegrationManagerMajorHeaderText(packageComponentAction.headerTitle);
            version = new IntegrationManagerVersion(
                $"Current Version: {currentlyInstalledPackageVersion}", 200);

            var upgradeText = "Your Unity Package is up to date.";
            status = AdapterData.Adapter.Status.UpToDate;

            if (IsPackageUpgradable(currentlyInstalledPackageVersion, latestPackageVersion))
            {
                upgradeText = "Upgrade the Unity Package integration for LevelPlay. " +
                    "Upgrading gives you access to the latest Network SDK versions.";
                status = AdapterData.Adapter.Status.UpdateAvailable;
            }

            normalText = new IntegrationManagerNormalText(upgradeText, k_updateTextFontSize);

            button = new IntegrationManagerButton(
                IntegrationManagerUIUtils.GetButtonText(status, latestPackageVersion),
                string.Empty,
                packageComponentAction.DownloadAction);
            dividerDarkColor = new Color(0.2f, 0.2f, 0.2f);
            dividerLightColor = new Color(0.0f, 0.0f, 0.0f, 0.15f);
        }

        public void UpdateDownloadAction((string headerTitle, Action DownloadAction) packageComponent)
        {
            majorHeaderText = new IntegrationManagerMajorHeaderText(packageComponent.headerTitle);
            button = new IntegrationManagerButton(
                IntegrationManagerUIUtils.GetButtonText(status, latestPackageVersion),
                string.Empty,
                packageComponent.DownloadAction);
        }

        public void Draw()
        {
            GUI.enabled = IsPackageUpgradable(currentlyInstalledPackageVersion, latestPackageVersion);
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                {
                    majorHeaderText.Draw();
                    GUILayout.FlexibleSpace();
                    version.Draw();
                    button.Draw();
                    GUILayout.Space(10);
                }
                GUILayout.Space(5);
                normalText.Draw();
                GUILayout.Space(10);
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            GUI.enabled = true;
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 3),
                EditorGUIUtility.isProSkin ? dividerDarkColor : dividerLightColor);
        }

        static bool IsPackageUpgradable(string currentlyInstalledPackageVersion, string latestPackageVersion)
        {
            var current = Version.Parse(currentlyInstalledPackageVersion);
            var latest = Version.Parse(latestPackageVersion);

            return current < latest;
        }
    }
}
#endif
