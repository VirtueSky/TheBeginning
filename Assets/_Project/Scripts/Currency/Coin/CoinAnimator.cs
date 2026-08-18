using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Audio;

namespace TheBeginning.Currency
{
    public class CoinAnimator : BaseCurrencyAnimator
    {
        [FormerlySerializedAs("iconCoinPrefab")]
        [SerializeField] private GameObject coinPrefab;
        [FormerlySerializedAs("soundCoinMove")]
        [SerializeField] private SoundData coinCollectSound;
        [SerializeField] private SoundData coinSpawnSound;

        protected override GameObject AnimationPrefab => coinPrefab;
        protected override SoundData CollectSound => coinCollectSound;
        protected override SoundData SpawnSound => coinSpawnSound;
    }
}
