using UnityEngine;
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
    // public void Load(LoadSceneData data)
    // {
    //     if (data.isWaiting)
    //     {
    //         _operation = SceneManager.LoadSceneAsync(data.sceneName, data.loadSceneMode);
    //         _operation.allowSceneActivation = false;
    //         StartCoroutine(Wait(data.timeLoad, data.loadCondition));
    //     }
    //     else
    //     {
    //         SceneManager.LoadScene(data.sceneName, data.loadSceneMode);
    //     }
    // }
    //
    // public void Unload(string sceneName)
    // {
    //     SceneManager.UnloadScene(sceneName);
    // }

    public void ChangeScene(string sceneName)
    {
        foreach (var scene in GetAllLoadedScene())
        {
            if (!scene.name.Equals(Constant.SERVICE_SCENE))
            {
                SceneManager.UnloadScene(scene.name);
            }
        }

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += operation =>
        {
            if (operation.isDone)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
        };
    }


    // IEnumerator Wait(float time, Func<bool> condition)
    // {
    //     yield return new WaitForSeconds(time);
    //     if (condition != null)
    //     {
    //         yield return new WaitUntil(condition);
    //     }
    //
    //     _operation.allowSceneActivation = true;
    // }

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