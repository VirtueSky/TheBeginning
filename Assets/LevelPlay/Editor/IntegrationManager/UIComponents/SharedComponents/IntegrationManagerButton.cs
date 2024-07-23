using System;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerButton : IDrawable
    {
        readonly Action _onClick;
        readonly GUIContent _guiContent;
        readonly RectOffset _padding;

        public IntegrationManagerButton(
            string buttonText,
            string tooltip,
            Action onClick)
        {
            _onClick = onClick;
            _padding = new RectOffset(11, 12, 1, 1);
            _guiContent = new GUIContent
            {
                text = buttonText,
                tooltip = tooltip
            };
        }

        public void Draw()
        {
            var buttonStyle = GUI.skin.button;
            buttonStyle.padding = _padding;
            var btn = GUILayout.Button(_guiContent, buttonStyle,
                GUILayout.ExpandWidth(true), GUILayout.MinWidth(80));

            if (btn)
            {
                _onClick.Invoke();
            }
        }
    }
}
