using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class MessageDisplayComponent: IDrawable
    {
        readonly string _messageData;
        public MessageDisplayComponent(string message)
        {
            _messageData = message;
        }

        public void Draw()
        {
            GUI.enabled = true;
            if (!string.IsNullOrWhiteSpace(_messageData))
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
                {
                    GUILayout.Space(4);
                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
                    {
                        GUILayout.Space(12);
                        EditorGUILayout.SelectableLabel(_messageData, EditorStyles.textField, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        GUILayout.Space(10);
                    }
                }
                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(false)))
                {
                    GUILayout.Space(15);
                }
            }
        }
    }
}
