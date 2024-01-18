using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Events;

namespace TheBeginning.Custom_Scriptable_Event
{
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
        public LoadSceneMode loadSceneMode;
        public Func<bool> loadCondition;

        public LoadSceneData(bool _isWaiting, string _sceneName, float _timeLoad = 2.5f,
            Func<bool> _loadCondition = null, LoadSceneMode _loadSceneMode = LoadSceneMode.Single)
        {
            isWaiting = _isWaiting;
            sceneName = _sceneName;
            timeLoad = _timeLoad;
            loadSceneMode = _loadSceneMode;
            loadCondition = _loadCondition;
        }
    }
}