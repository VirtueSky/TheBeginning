using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;

namespace TheBeginning.Services
{
    [HideMonoScript]
    public class PoolInitialization : ServiceInitialization
    {
        [SerializeField] private Pools pools;

        public override void Initialization()
        {
            pools.Initialize();
        }
    }
}