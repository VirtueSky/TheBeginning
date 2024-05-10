using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Coin Generate/Add Target To Event",
    fileName = "add_target_to_coin_generate_event")]
[EditorIcon("scriptable_event")]
public class AddTargetToCoinGenerateEvent : BaseEvent<GameObject>
{
}