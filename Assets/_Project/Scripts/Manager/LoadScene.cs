using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Core;

public class LoadScene : BaseMono
{
    private AsyncOperation _operation;

    public void Load(LoadSceneData data)
    {
        if (data.isWaiting)
        {
            _operation = SceneManager.LoadSceneAsync(data.sceneName);
            _operation.allowSceneActivation = false;
            StartCoroutine(Wait(data.timeLoad, data.loadCondition));
        }
        else
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }

    IEnumerator Wait(float time, Func<bool> condition)
    {
        yield return new WaitForSeconds(time);
        if (condition != null)
        {
            yield return new WaitUntil(condition);
        }

        _operation.allowSceneActivation = true;
    }
}