using Cysharp.Threading.Tasks;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace TheBeginning.LevelSystem
{
    [CreateAssetMenu(menuName = "Event Custom/Event Load Level", fileName = "event_load_level")]
    [EditorIcon("scriptable_event")]
    public class EventLoadLevel : EventNoParamResult<Level>
    {
    }
}