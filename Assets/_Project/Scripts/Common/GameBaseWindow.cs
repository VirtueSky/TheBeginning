#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class GameBaseWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.EndHorizontal();
    }

    [MenuItem("GameBase/Open Scene/Launcher Scene %F1")]
    public static void OpenLauncherScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.LAUNCHER_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Open Scene/Service Scene %F2")]
    public static void OpenServiceScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.SERVICE_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Open Scene/Home Scene %F3")]
    public static void OpenHomeScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.HOME_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Open Scene/Game Scene %F4")]
    public static void OpenGameScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.GAME_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Play Launcher")]
    public static void PlayLauncher()
    {
        OpenLauncherScene();
        EditorApplication.isPlaying = true;
    }
}
#endif