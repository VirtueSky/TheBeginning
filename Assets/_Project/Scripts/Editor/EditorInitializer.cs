#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class EditorInitializer
{
    static EditorInitializer()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private const string serviceScenePath = "Assets/_Project/Scenes/Service.unity";
    const string SaveSceneKey = "SaveSceneKey";

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                var activeScenePath = EditorSceneManager.GetActiveScene().path;
                if (!activeScenePath.Equals(serviceScenePath))
                {
                    EditorPrefs.SetString(SaveSceneKey, activeScenePath);
                    EditorApplication.isPlaying = false;
                    EditorApplication.delayCall += () =>
                    {
                        EditorSceneManager.OpenScene(serviceScenePath);
                        EditorApplication.isPlaying = true;
                    };
                }

                break;
            case PlayModeStateChange.EnteredEditMode:
                if (EditorPrefs.HasKey(SaveSceneKey))
                {
                    string previousScenePath = EditorPrefs.GetString(SaveSceneKey);
                    EditorPrefs.DeleteKey(SaveSceneKey);
                    EditorApplication.delayCall += () => { EditorSceneManager.OpenScene(previousScenePath); };
                }

                break;
        }
    }
}
#endif