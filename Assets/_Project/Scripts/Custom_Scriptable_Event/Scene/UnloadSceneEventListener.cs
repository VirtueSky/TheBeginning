using UnityEngine.Events;
using VirtueSky.Events;

namespace TheBeginning.Custom_Scriptable_Event
{
    public class UnloadSceneEventListener : BaseEventListener<UnloadSceneData, UnloadSceneEvent,
        UnityEvent<UnloadSceneData>>
    {
    }
}