using UnityEngine;
using VirtueSky.Inspector;

namespace TheBeginning.Services
{
    [HideMonoScript]
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
        [ContextMenu("GetServiceInitialization")]
        void GetServiceInitialization()
        {
            serviceInitializations = FindObjectsByType<ServiceInitialization>(FindObjectsSortMode.None);
        }
#endif
    }
}