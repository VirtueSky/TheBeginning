using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Event Get Transform Current Level",
    fileName = "event_get_transform_current_level")]
[EditorIcon("scriptable_event")]
public class EventGetTransformCurrentLevel : EventNoParamResult<Transform>
{
}