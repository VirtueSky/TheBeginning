using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace TheBeginning.LevelSystem
{
    [CreateAssetMenu(menuName = "Event Custom/Event Get Current Level", fileName = "event_get_current_level")]
    [EditorIcon("scriptable_event")]
    public class EventGetCurrentLevel : EventNoParamResult<Level>
    {
    }
}