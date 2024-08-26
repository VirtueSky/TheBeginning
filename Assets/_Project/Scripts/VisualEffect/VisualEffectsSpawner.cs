using PrimeTween;
using TheBeginning.Config;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;

[HideMonoScript]
public class VisualEffectsSpawner : BaseMono
{
    [SerializeField] private VisualEffectConfig visualEffectConfig;
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
            //GameObject effect = pools.Spawn(randomEffect, data.parent, false);
            GameObject effect = randomEffect.Spawn(data.parent, false);
            effect.transform.position = data.position;
            effect.transform.localScale =
                (data.localScale == default) ? Vector3.one : data.localScale;

            if (data.isDestroyedOnEnd)
            {
                App.Delay(data.timeDestroy, () =>
                {
                    if (effect != null) effect.DeSpawn();
                });
            }
        }
    }
}