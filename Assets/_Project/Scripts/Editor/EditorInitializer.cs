#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EditorInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async void RuntimeEditorInitialize()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case Constant.SERVICE_SCENE:
                return;
            case Constant.GAME_SCENE:
                SceneManager.LoadScene(Constant.SERVICE_SCENE);
                break;
        }
    }
}
#endif