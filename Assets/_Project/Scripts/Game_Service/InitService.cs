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


        private void Awake()
        {
            Application.targetFrameRate = 60;
            pools.Initialize();
            Vibration.Init();
        }
    }
}