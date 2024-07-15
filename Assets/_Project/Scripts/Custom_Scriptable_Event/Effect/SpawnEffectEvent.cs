using System;
using TheBeginning.Config;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/Spawn Effect Event", fileName = "spawn_effect_event")]
[EditorIcon("scriptable_event")]
public class SpawnEffectEvent : BaseEvent<SpawnEffectData>
{
}

[Serializable]
public class SpawnEffectData
{
    public VisualEffectType visualEffectType;
    public Transform parent;
    public Vector3 position;
    public Vector3 localScale;
    public bool isDestroyedOnEnd;
    public float timeDestroy;

    public SpawnEffectData(VisualEffectType visualEffectType, Transform parent, Vector3 position,
        Vector3 localScale = default,
        bool isDestroyedOnEnd = true, float timeDestroy = 3f)
    {
        this.visualEffectType = visualEffectType;
        this.parent = parent;
        this.position = position;
        this.localScale = localScale;
        this.isDestroyedOnEnd = isDestroyedOnEnd;
        this.timeDestroy = timeDestroy;
    }
}