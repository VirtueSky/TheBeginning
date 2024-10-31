using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Vfx
{
    [CreateAssetMenu(menuName = "Sunflower/Vfx/Vfx Data", fileName = "vfx_data")]
    [EditorIcon("scriptable_particle_system")]
    public class VfxData : ScriptableObject
    {
        [SerializeField] private float timeDestroy = 3;

        [SerializeField] private List<GameObject> listVfx;

        public float TimeDestroy => timeDestroy;

        public GameObject GetVfxRandom() => listVfx.PickRandom();

        public GameObject GetVfxByIndex(int index) => listVfx[Mathf.Clamp(index, 0, listVfx.Count - 1)];
    }
}