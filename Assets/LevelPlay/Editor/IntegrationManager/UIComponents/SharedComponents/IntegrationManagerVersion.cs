#if LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerVersion: IDrawable
    {
        readonly string text;
        readonly GUIStyle style;
        readonly GUILayoutOption layout;
        Rect rect;

        public IntegrationManagerVersion(string text, float xOffset)
        {
            this.text = text;
            style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Normal,
                fontSize = 10,
                alignment = TextAnchor.MiddleLeft
            };

            rect = new Rect(IronSourceDependenciesManager.k_Width / 3 + xOffset,
                0, 200, 20);
        }

        public void Draw()
        {
            rect.y = GUILayoutUtility.GetLastRect().y;
            EditorGUI.LabelField(rect, text, style);
        }
    }
}
#endif
