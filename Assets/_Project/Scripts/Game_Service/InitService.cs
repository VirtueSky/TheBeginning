using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Notifications;
using VirtueSky.ObjectPooling;
using VirtueSky.Vibration;

namespace TheBeginning.Services
{
    public class InitService : MonoBehaviour
    {
        [SerializeField] private Pools pools;
        [SerializeField] List<BaseMono> listServersInitialized;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            pools.Initialize();
            Vibration.Init();
            foreach (var mono in listServersInitialized)
            {
                var monoService = Instantiate(mono);
                monoService.Initialize();
            }
        }
    }
}