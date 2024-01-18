using System;
using UnityEngine;
using VirtueSky.Events;

namespace TheBeginning.Custom_Scriptable_Event
{
    [CreateAssetMenu(menuName = "Event Custom/Unload Screen Event",
        fileName = "unload_scene_event")]
    public class UnloadSceneEvent : BaseEvent<UnloadSceneData>
    {
    }

    [Serializable]
    public class UnloadSceneData
    {
        private string sceneName;

        public UnloadSceneData(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }
}