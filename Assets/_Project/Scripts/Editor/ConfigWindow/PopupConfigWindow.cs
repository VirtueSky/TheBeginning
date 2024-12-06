#if UNITY_EDITOR
using TheBeginning.UI;
using UnityEditor;
using UnityEngine;
using VirtueSky.ControlPanel.Editor;
using VirtueSky.UtilsEditor;

public class PopupConfigWindow
{
    private static Vector2 _scrollPosition;
    private static UnityEditor.Editor _editor;
    private static PopupSettings _config;
    private static Vector2 scroll = Vector2.zero;

    public static void OnEnable()
    {
        Init();
    }

    private static void Init()
    {
        if (_editor != null) _editor = null;
        _config = CreateAsset.GetScriptableAsset<PopupSettings>();
        _editor = UnityEditor.Editor.CreateEditor(_config);
    }

    public static void Draw()
    {
        GUILayout.BeginVertical();
        CPUtility.DrawHeader("Popup Settings");
        CPUtility.GuiLine();
        GUILayout.Space(10);
        scroll = EditorGUILayout.BeginScrollView(scroll);
        if (_config == null)
        {
            if (GUILayout.Button("Create PopupSettings"))
            {
                _config = CreateAsset.CreateAndGetScriptableAsset<PopupSettings>("Assets/_project/Config", useDefaultPath: false);
                Init();
            }
        }
        else
        {
            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                    MessageType.Error);
                return;
            }

            _editor.OnInspectorGUI();
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
#endif