using JetBrains.Annotations;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents
{
    class IntegrationManagerRecommendedBox: IDrawable
    {
        readonly GUIStyle style;
        readonly Texture2D image;

        public IntegrationManagerRecommendedBox([CanBeNull] Texture2D recommendedIcon)
        {
            style = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                margin = new RectOffset(5, 0, 0, 0),
                padding = new RectOffset(0,0,0,-20),
                fixedWidth = 75
            };

            image = recommendedIcon;
        }

        public void Draw()
        {
            GUILayout.Label(image, style);
        }
    }
}
