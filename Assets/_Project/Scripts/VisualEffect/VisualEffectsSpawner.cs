using PrimeTween;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;


public class VisualEffectsSpawner : BaseMono
{
    [SerializeField] private VisualEffectConfig visualEffectConfig;
    [SerializeField] private Pools pools;
    [SerializeField] private SpawnEffectEvent spawnEffectEvent;

    public override void OnEnable()
    {
        base.OnEnable();
        spawnEffectEvent.AddListener(SpawnEffect);
    }

    public void SpawnEffect(SpawnEffectData data)
    {
        VisualEffectData visualEffectData = visualEffectConfig.GetVisualEffectDataByType(data.visualEffectType);
        if (visualEffectData != null)
        {
            GameObject randomEffect = visualEffectData.GetRandomEffect();
            GameObject effect = pools.Spawn(randomEffect, data.parent, false);
            effect.transform.position = data.position;
            effect.transform.localScale =
                (data.localScale == default) ? Vector3.one : data.localScale;

            if (data.isDestroyedOnEnd)
            {
                Tween.Delay(data.timeDestroy, () => pools.DeSpawn(effect));
            }
        }
    }
}