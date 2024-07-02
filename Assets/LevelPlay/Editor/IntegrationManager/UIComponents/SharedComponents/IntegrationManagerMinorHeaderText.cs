#if LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerMinorHeaderText: IDrawable
    {
        readonly string name;
        readonly GUIStyle style;

        public IntegrationManagerMinorHeaderText(string name)
        {
            this.name = name;
            style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                fixedHeight = 20,
                stretchWidth = true,
                fixedWidth = IronSourceDependenciesManager.k_Width / 3  - 110,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public void Draw()
        {
            GUILayout.Label(name, style);
        }
    }
}
#endif
