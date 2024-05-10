using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Coin Generate/Remove Target To Event",
    fileName = "remove_target_to_coin_generate_event")]
[EditorIcon("scriptable_event")]
public class RemoveTargetToCoinGenerateEvent : BaseEvent<GameObject>
{
}