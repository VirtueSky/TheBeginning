using TheBeginning.LevelSystem;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Event Level", fileName = "event_level")]
[EditorIcon("scriptable_event")]
public class EventLevel : BaseEvent<Level>
{
}