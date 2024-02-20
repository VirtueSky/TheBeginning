using UnityEngine;
using VirtueSky.ObjectPooling;
using VirtueSky.Vibration;

namespace TheBeginning.Services
{
    public class InitService : MonoBehaviour
    {
        [SerializeField] private Pools pools;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private ItemConfig itemConfig;

        private void Awake()
        {
            Application.targetFrameRate = (int)gameConfig.targetFrameRate;
            pools.Initialize();
            Vibration.Init();
            itemConfig.Initialize();
        }
    }
}