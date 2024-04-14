using PrimeTween;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(Camera))]
    public class CameraShakerComponent : BaseMono
    {
        [SerializeField] private Camera cameraShaker;
        [SerializeField] private float durationPosition = .3f;
        [SerializeField] private float durationRotation = .3f;
        [SerializeField] private Vector3 positionStrength;
        [SerializeField] private Vector3 rotationStrength;
        private Tween tween;

        public void CameraShake()
        {
            tween.Complete();
            tween = cameraShaker.DOShakePosition(durationPosition, positionStrength);
            tween = cameraShaker.DOShakeRotation(durationRotation, rotationStrength);
        }

        public void CameraShake(float _durationPosition, float _durationRotation, Vector3 _positionStrength,
            Vector3 _rotationStrength)
        {
            tween.Complete();
            tween = cameraShaker.DOShakePosition(_durationPosition, _positionStrength);
            tween = cameraShaker.DOShakeRotation(_durationRotation, _rotationStrength);
        }
#if UNITY_EDITOR
        private void Reset()
        {
            if (cameraShaker == null)
            {
                cameraShaker = GetComponent<Camera>();
            }
        }
#endif
    }
}