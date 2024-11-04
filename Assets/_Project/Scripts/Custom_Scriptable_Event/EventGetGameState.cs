using TheBeginning.Game;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Event Get GameState", fileName = "event_get_game_state")]
[EditorIcon("scriptable_event")]
public class EventGetGameState : EventNoParamResult<GameState>
{
}