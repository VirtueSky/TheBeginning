#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Threading.Tasks;

public static class EditorInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static async void RuntimeEditorInitialize()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case Constant.LAUNCHER_SCENE:
                return;
            case Constant.SERVICE_SCENE:
                await SceneManager.LoadSceneAsync(Constant.LAUNCHER_SCENE);
                break;
            case Constant.GAME_SCENE:
                await SceneManager.LoadSceneAsync(Constant.LAUNCHER_SCENE);
                break;
        }
    }
}
#endif