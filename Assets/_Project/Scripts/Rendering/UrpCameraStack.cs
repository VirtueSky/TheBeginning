using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TheBeginning.Rendering
{
    /// <summary>Connects cameras loaded in separate Addressable scenes without cross-scene references.</summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera), typeof(UniversalAdditionalCameraData))]
    public sealed class UrpCameraStack : MonoBehaviour
    {
        [SerializeField] private bool overlay;

        private static readonly List<UrpCameraStack> Cameras = new List<UrpCameraStack>();
        private Camera cachedCamera;
        private UniversalAdditionalCameraData cameraData;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRegistry() => Cameras.Clear();

        private void OnEnable()
        {
            cachedCamera = GetComponent<Camera>();
            cameraData = GetComponent<UniversalAdditionalCameraData>();
            if (!Cameras.Contains(this)) Cameras.Add(this);
            Rebuild();
        }

        private void OnDisable()
        {
            Cameras.Remove(this);
            if (cameraData != null && cameraData.renderType == CameraRenderType.Base)
                cameraData.cameraStack.Clear();
            Rebuild();
        }

        private static void Rebuild()
        {
            Cameras.RemoveAll(item => item == null);
            Cameras.Sort((left, right) => left.cachedCamera.depth.CompareTo(right.cachedCamera.depth));

            UrpCameraStack baseCamera = null;
            foreach (var item in Cameras)
            {
                if (item.cameraData.renderType == CameraRenderType.Base)
                    item.cameraData.cameraStack.Clear();
                item.cameraData.renderType = item.overlay ? CameraRenderType.Overlay : CameraRenderType.Base;
                if (!item.overlay && item.cachedCamera.isActiveAndEnabled)
                    baseCamera = item;
            }

            // A UI-only scene can also be opened on its own in the Editor.
            if (baseCamera == null)
            {
                foreach (var item in Cameras)
                {
                    if (!item.cachedCamera.isActiveAndEnabled) continue;
                    baseCamera = item;
                    item.cameraData.renderType = CameraRenderType.Base;
                    break;
                }
            }
            if (baseCamera == null) return;
            foreach (var item in Cameras)
            {
                if (item != baseCamera && item.overlay && item.cachedCamera.isActiveAndEnabled &&
                    item.cachedCamera.targetDisplay == baseCamera.cachedCamera.targetDisplay)
                    baseCamera.cameraData.cameraStack.Add(item.cachedCamera);
            }
        }
    }
}
