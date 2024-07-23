using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerNormalText : IDrawable
    {
        readonly string text;
        readonly GUIStyle style;

        public IntegrationManagerNormalText(string text, int fontSize)
        {
            this.text = text;
            style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Normal,
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(0, 0, 0, 0)
            };
        }

        public void Draw()
        {
            GUILayout.Label(text, style);
        }
    }
}
