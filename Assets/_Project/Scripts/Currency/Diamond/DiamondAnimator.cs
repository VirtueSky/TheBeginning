using UnityEngine;
using VirtueSky.Audio;

namespace TheBeginning.Currency
{
    public class DiamondAnimator : BaseCurrencyAnimator
    {
        [SerializeField] private GameObject diamondPrefab;
        [SerializeField] private SoundData diamondCollectSound;

        protected override GameObject AnimationPrefab => diamondPrefab;
        protected override SoundData CollectSound => diamondCollectSound;
    }
}
