#if UNITY_EDITOR
using DG.Tweening.Plugins.Core.PathCore;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Windows;
using UnityEngine.WSA;
using VirtueSky.DataStorage;
using File = System.IO.File;
using Path = System.IO.Path;

public class GameBase : EditorWindow
{
    void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.EndHorizontal();
    }

    [MenuItem("GameBase/Switch Debug %`")]
    public static void SwitchDebug()
    {
        Data.IsTesting = !Data.IsTesting;
        Debug.Log($"<color=Green>Data.IsTesting = {Data.IsTesting}</color>");
    }

    [MenuItem("GameBase/Open Scene/Loading Scene %F1")]
    public static void PlayFromLoadingScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.LOADING_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Open Scene/Home Scene %F2")]
    public static void PlayFromHomeScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.HOME_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    [MenuItem("GameBase/Open Scene/Gameplay Scene %F3")]
    public static void PlayFromGamePlayScene()
    {
        EditorSceneManager.OpenScene($"Assets/_Project/Scenes/{Constant.GAMEPLAY_SCENE}.unity");
        Debug.Log($"<color=Green>Change scene succeed</color>");
    }

    // [MenuItem("GameBase/Data/Add 100k Money")]
    // public static void Add100kMoney()
    // {
    //     Data.CurrencyTotal += 100000;
    //     Debug.Log($"<color=Green>Add 100k coin succeed</color>");
    // }
}
#endif