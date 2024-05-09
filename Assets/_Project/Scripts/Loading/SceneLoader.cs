using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using VirtueSky.Core;
using VirtueSky.Events;
using TheBeginning;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : BaseMono
{
    [SerializeField] private StringEvent changeSceneEvent;

    public override void OnEnable()
    {
        base.OnEnable();
        changeSceneEvent.AddListener(ChangeScene);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        changeSceneEvent.RemoveListener(ChangeScene);
    }

    public void ChangeScene(string sceneName)
    {
        foreach (var scene in GetAllLoadedScene())
        {
            if (!scene.name.Equals(Constant.SERVICE_SCENE))
            {
                if (Utility.sceneHolder.ContainsKey(scene.name))
                {
                    Addressables.UnloadSceneAsync(Utility.sceneHolder[scene.name]);
                    Utility.sceneHolder.Remove(scene.name);
                }
                else
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
        }

        Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive).Completed += OnAdditiveSceneLoaded;
    }

    void OnAdditiveSceneLoaded(AsyncOperationHandle<SceneInstance> scene)
    {
        if (scene.Status == AsyncOperationStatus.Succeeded)
        {
            string sceneName = scene.Result.Scene.name;
            Utility.sceneHolder.Add(sceneName, scene);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }

    private Scene[] GetAllLoadedScene()
    {
        int countLoaded = SceneManager.sceneCount;
        var loadedScenes = new Scene[countLoaded];

        for (var i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        return loadedScenes;
    }
}