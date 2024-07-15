using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Threading.Tasks;

namespace TheBeginning.LevelSystem
{
    [CreateAssetMenu(menuName = "Event Custom/Event Load Level", fileName = "event_load_level")]
    [EditorIcon("scriptable_event")]
    public class EventLoadLevel : EventNoParamResult<UniTask<Level>>
    {
    }
}