#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class EditorInitializer
{
    private const string enableAutoSwitchKey = "EditorInitializer_Enable";
    private const string menuPath = "The Beginning/Auto Switch Service Scene";
    private const string serviceScenePath = "Assets/_Project/Scenes/Service.unity";
    private const string saveSceneKey = "SaveSceneKey";

    static EditorInitializer()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem(menuPath)]
    private static void ToggleEnable()
    {
        bool enabled = !IsEnabled;
        EditorPrefs.SetBool(enableAutoSwitchKey, enabled);
        Menu.SetChecked(menuPath, enabled);
    }

    [MenuItem(menuPath, true)]
    private static bool ToggleEnableValidate()
    {
        Menu.SetChecked(menuPath, IsEnabled);
        return true;
    }

    private static bool IsEnabled => EditorPrefs.GetBool(enableAutoSwitchKey, true);


    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!IsEnabled) return;
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                var activeScenePath = EditorSceneManager.GetActiveScene().path;
                if (!activeScenePath.Equals(serviceScenePath))
                {
                    EditorPrefs.SetString(saveSceneKey, activeScenePath);
                    EditorApplication.isPlaying = false;
                    EditorApplication.delayCall += () =>
                    {
                        EditorSceneManager.OpenScene(serviceScenePath);
                        EditorApplication.isPlaying = true;
                    };
                }

                break;
            case PlayModeStateChange.EnteredEditMode:
                if (EditorPrefs.HasKey(saveSceneKey))
                {
                    string previousScenePath = EditorPrefs.GetString(saveSceneKey);
                    EditorPrefs.DeleteKey(saveSceneKey);
                    EditorApplication.delayCall += () => { EditorSceneManager.OpenScene(previousScenePath); };
                }

                break;
        }
    }
}
#endif