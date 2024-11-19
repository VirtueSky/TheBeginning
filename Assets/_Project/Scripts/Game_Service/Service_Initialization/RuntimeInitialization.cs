using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace TheBeginning.Services
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class RuntimeInitialization : MonoBehaviour
    {
        [SerializeField] private ServiceInitialization[] serviceInitializations;

        private void Awake()
        {
            foreach (var serviceInitialization in serviceInitializations)
            {
                serviceInitialization.Initialization();
            }
        }
#if UNITY_EDITOR
        [Button]
        void GetServiceInitialization()
        {
            serviceInitializations = GetComponents<ServiceInitialization>();
        }
#endif
    }
}