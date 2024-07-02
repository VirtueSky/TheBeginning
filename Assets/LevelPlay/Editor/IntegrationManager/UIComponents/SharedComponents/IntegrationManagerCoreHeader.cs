#if LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerCoreHeader: IDrawable
    {
        readonly IntegrationManagerMajorHeaderText majorHeaderText;
        readonly IntegrationManagerMinorHeaderText currentVersion;
        readonly IntegrationManagerMinorHeaderText latestVersion;

        public IntegrationManagerCoreHeader(string mainTitle)
        {
            majorHeaderText = new IntegrationManagerMajorHeaderText(mainTitle);
            currentVersion = new IntegrationManagerMinorHeaderText("Current Version");
            latestVersion = new IntegrationManagerMinorHeaderText("Latest Version");
        }

        public void Draw()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
            {
                majorHeaderText.Draw();
                GUILayout.Space(54);
                currentVersion.Draw();
                latestVersion.Draw();
            }
        }
    }
}
#endif
