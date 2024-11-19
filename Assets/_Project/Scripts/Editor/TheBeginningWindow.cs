#if UNITY_EDITOR
using System.Linq;
using TheBeginning.Config;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;


public class TheBeginningWindow : EditorWindow
{
    private Editor _editorGameConfig;
    private GameConfig _gameConfig;
    private Vector2 _scrollPosition;

    [MenuItem("The Beginning/Open GameConfig %`", priority = 1)]
    public static void OpenGameConfigWindow()
    {
        GameConfig gameConfig = FileExtension.FindAssetAtFolder<GameConfig>(new string[] { "Assets" }).FirstOrDefault();
        TheBeginningWindow window = GetWindow<TheBeginningWindow>("Game Config");
        window._gameConfig = gameConfig;
        if (window == null)
        {
            Debug.LogError("Couldn't open the ads settings window!");
            return;
        }

        window.minSize = new Vector2(350, 250);
        window.Show();
    }

    private void OnGUI()
    {
        // EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
        //     GameDataEditor.ColorBackgroundRectWindowSunflower.ToColor());
        // GUI.contentColor = GameDataEditor.ColorTextContentWindowSunflower.ToColor();
        // GUI.backgroundColor = GameDataEditor.ColorContentWindowSunflower.ToColor();
        if (_editorGameConfig == null)
        {
            _editorGameConfig = UnityEditor.Editor.CreateEditor(_gameConfig);
        }

        if (_editorGameConfig == null)
        {
            EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                MessageType.Error);
            return;
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        _editorGameConfig.OnInspectorGUI();
        GUILayout.Space(10);
        EditorGUILayout.EndScrollView();
    }


    [MenuItem("The Beginning/Open Scene Service %F1", priority = 200)]
    public static void OpenServiceScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.SERVICE_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("The Beginning/Open Scene Game %F2", priority = 201)]
    public static void OpenGameScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.GAME_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }
}
#endif