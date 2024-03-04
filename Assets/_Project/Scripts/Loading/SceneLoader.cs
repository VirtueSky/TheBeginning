using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VirtueSky.Core;
using VirtueSky.Events;


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
                SceneManager.UnloadScene(scene.name);
            }
        }

        Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive).Completed += handle =>
        {
            if (handle.IsDone)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
        };
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