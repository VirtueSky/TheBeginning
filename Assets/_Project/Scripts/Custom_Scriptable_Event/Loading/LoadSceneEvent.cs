using System;
using UnityEngine;
using VirtueSky.Events;

[CreateAssetMenu(menuName = "Event Custom/Load Screen Event", fileName = "load_scene_event")]
public class LoadSceneEvent : BaseEvent<LoadSceneData>
{
}

[Serializable]
public class LoadSceneData
{
    public bool isWaiting;
    public string sceneName;
    public float timeLoad;
    public Func<bool> loadCondition;

    public LoadSceneData(bool _isWaiting, string _sceneName, float _timeLoad = 2.5f, Func<bool> _loadCondition = null)
    {
        isWaiting = _isWaiting;
        sceneName = _sceneName;
        timeLoad = _timeLoad;
        loadCondition = _loadCondition;
    }
}