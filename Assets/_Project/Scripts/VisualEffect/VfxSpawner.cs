using UnityEngine;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Vfx
{
    public static class VfxSpawner
    {
        public static void Spawn(this VfxData vfxData, Transform parent, Vector3 position, Quaternion quaternion, Vector3 localScale, int index = -1)
        {
            if (vfxData == null) return;
            GameObject vfxPrefab = index < 0 ? vfxData.GetVfxRandom() : vfxData.GetVfxByIndex(index);
            GameObject vfxSpawn = vfxPrefab.Spawn(position, quaternion, parent, false);
            vfxSpawn.transform.localScale = localScale;
            App.Delay(vfxData.TimeDestroy, () => vfxSpawn.DeSpawn());
        }

        public static void Spawn(this VfxData vfxData, Transform parent, Vector3 position, Quaternion quaternion, int index = -1)
        {
            if (vfxData == null) return;
            GameObject vfxPrefab = index < 0 ? vfxData.GetVfxRandom() : vfxData.GetVfxByIndex(index);
            GameObject vfxSpawn = vfxPrefab.Spawn(position, quaternion, parent, false);
            App.Delay(vfxData.TimeDestroy, () => vfxSpawn.DeSpawn());
        }

        public static void Spawn(this VfxData vfxData, Transform parent, int index = -1)
        {
            if (vfxData == null) return;
            GameObject vfxPrefab = index < 0 ? vfxData.GetVfxRandom() : vfxData.GetVfxByIndex(index);
            GameObject vfxSpawn = vfxPrefab.Spawn(parent, false);
            App.Delay(vfxData.TimeDestroy, () => vfxSpawn.DeSpawn());
        }
    }
}