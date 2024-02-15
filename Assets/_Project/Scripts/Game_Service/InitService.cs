using UnityEngine;
using VirtueSky.ObjectPooling;
using VirtueSky.Vibration;

namespace TheBeginning.Services
{
    public class InitService : MonoBehaviour
    {
        [SerializeField] private Pools pools;
        [SerializeField] private GameConfig gameConfig;

        private void Awake()
        {
            Application.targetFrameRate = (int)gameConfig.targetFrameRate;
            pools.Initialize();
            Vibration.Init();
        }
    }
}