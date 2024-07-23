#if LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerMajorHeaderText: IDrawable
    {
        readonly string name;
        readonly GUIStyle style;

        public IntegrationManagerMajorHeaderText(string name)
        {
            this.name = name;
            style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14,
                fixedHeight = 20,
                stretchWidth = true,
                alignment = TextAnchor.MiddleLeft,
                fixedWidth = name.Length < 40 ? IronSourceDependenciesManager.k_Width / 3 + 5
                    : IronSourceDependenciesManager.k_Width / 3 + 300,
                margin = new RectOffset(0, 0, 0, 0)
            };
        }

        public void Draw()
        {
            GUILayout.Label(name, style);
        }
    }
}
#endif
