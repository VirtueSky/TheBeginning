using UnityEngine.Events;
using VirtueSky.Events;

namespace TheBeginning.Custom_Scriptable_Event
{
    public class LoadSceneEventListener : BaseEventListener<LoadSceneData, LoadSceneEvent,
        UnityEvent<LoadSceneData>>
    {
    }
}