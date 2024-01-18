#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class GameBase : EditorWindow
{
    void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.EndHorizontal();
    }

    // [MenuItem("GameBase/Switch Debug %`")]
    // public static void SwitchDebug()
    // {
    //     Data.IsTesting = !Data.IsTesting;
    //     Debug.Log($"<color=Green>Data.IsTesting = {Data.IsTesting}</color>");
    // }

    [MenuItem("GameBase/Open Scene/Loading Scene %F1")]
    public static void OpenLoadingScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.LOADING_SCENE}.unity");
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

    [MenuItem("GameBase/Open Scene/Gameplay Scene %F4")]
    public static void OpenGamePlayScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.GAMEPLAY_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Play Loading")]
    public static void PlayLoading()
    {
        OpenLoadingScene();
        EditorApplication.isPlaying = true;
    }
}
#endif