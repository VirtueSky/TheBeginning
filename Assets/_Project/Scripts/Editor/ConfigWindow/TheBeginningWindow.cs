#if UNITY_EDITOR
using System.Linq;
using TheBeginning.Config;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VirtueSky.ControlPanel.Editor;
using VirtueSky.UtilsEditor;


public class TheBeginningWindow : EditorWindow
{
    enum StateWindow
    {
        GameSettings,
        LevelSettings,
        PopupSettings
    }

    private StateWindow stateWindow;
    private Vector2 scrollButton = Vector2.zero;

    [MenuItem("The Beginning/Open GameConfig %`", priority = 1)]
    public static void OpenGameConfigWindow()
    {
        TheBeginningWindow window = GetWindow<TheBeginningWindow>("The Beginning");
        if (window == null)
        {
            Debug.LogError("Couldn't open the TheBeginning window!");
            return;
        }

        window.minSize = new Vector2(550, 500);
        window.Show();
    }

    private void OnEnable()
    {
        GameSettingsWindow.OnEnable();
        LevelSettingsWindow.OnEnable();
        PopupSettingsWindow.OnEnable();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        CPUtility.DrawHeader("TheBeginning", 17);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        Handles.color = Color.black;
        CPUtility.DrawCustomLine(4, new Vector2(0, 30), new Vector2(position.width, 30));
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(165));
        scrollButton = EditorGUILayout.BeginScrollView(scrollButton);
        DrawButton();
        EditorGUILayout.EndScrollView();
        CPUtility.DrawCustomLine(4, new Vector3(170, 30), new Vector3(170, position.height));
        GUILayout.EndVertical();
        GUILayout.Space(10);
        DrawContent();
        GUILayout.EndHorizontal();
    }

    private void DrawContent()
    {
        switch (stateWindow)
        {
            case StateWindow.GameSettings:
                GameSettingsWindow.Draw();
                break;
            case StateWindow.LevelSettings:
                LevelSettingsWindow.Draw();
                break;
            case StateWindow.PopupSettings:
                PopupSettingsWindow.Draw();
                break;
        }
    }

    private void DrawButton()
    {
        DrawButtonChooseState("Game Settings", StateWindow.GameSettings);
        DrawButtonChooseState("Level Settings", StateWindow.LevelSettings);
        DrawButtonChooseState("Popup Settings", StateWindow.PopupSettings);
    }

    void DrawButtonChooseState(string title, StateWindow _stateWindow)
    {
        bool clicked = GUILayout.Toggle(stateWindow == _stateWindow, title, GUI.skin.button,
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.Height(20));
        if (clicked && stateWindow != _stateWindow)
        {
            stateWindow = _stateWindow;
        }

        GUILayout.Space(2);
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