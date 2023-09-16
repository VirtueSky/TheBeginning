// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;
using UnityEngine.Events;

namespace Animancer.Examples.AnimatorControllers.GameKit
{
    /// <summary>A <see cref="CharacterState"/> which plays a "landing on the ground" animation.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/3d-game-kit/landing">3D Game Kit/Landing</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers.GameKit/LandingState
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Game Kit - Landing State")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(AnimatorControllers) + "." + nameof(GameKit) + "/" + nameof(LandingState))]
    public sealed class LandingState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private MixerTransition2D _SoftLanding;
        [SerializeField] private ClipTransition _HardLanding;
        [SerializeField, MetersPerSecond] private float _HardLandingForwardSpeed = 5;
        [SerializeField, MetersPerSecond] private float _HardLandingVerticalSpeed = -10;
        [SerializeField] private UnityEvent _PlayAudio;// See the Read Me.

        private bool _IsSoftLanding;

        /************************************************************************************************************************/

        private void Awake()
        {
            _SoftLanding.Events.OnEnd =
                _HardLanding.Events.OnEnd =
                () => Character.CheckMotionState();
        }

        /************************************************************************************************************************/

        public override bool CanEnterState => Character.IsGrounded;

        /************************************************************************************************************************/

        /// <summary>
        /// Performs either a hard or soft landing depending on the current speed (both horizontal and vertical).
        /// </summary>
        private void OnEnable()
        {
            Character.ForwardSpeed = Character.DesiredForwardSpeed;

            if (Character.VerticalSpeed <= _HardLandingVerticalSpeed &&
                Character.ForwardSpeed >= _HardLandingForwardSpeed)
            {
                _IsSoftLanding = false;
                Character.Animancer.Play(_HardLanding);
            }
            else
            {
                _IsSoftLanding = true;
                Character.Animancer.Play(_SoftLanding);
                _SoftLanding.State.Parameter = new Vector2(Character.ForwardSpeed, Character.VerticalSpeed);
            }

            // The landing sounds in the full 3D Game Kit have different variations based on the ground material, just
            // like the footstep sounds as discussed in the LocomotionState.

            _PlayAudio.Invoke();
        }

        /************************************************************************************************************************/

        public override bool FullMovementControl => _IsSoftLanding;

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            if (!Character.IsGrounded &&
                Character.StateMachine.TrySetState(Character.Airborne))
                return;

            Character.UpdateSpeedControl();

            if (_IsSoftLanding)
            {
                // Update the horizontal speed but keep the initial vertical speed from when you first landed.
                var parameter = _SoftLanding.State.Parameter;
                parameter.x = Character.ForwardSpeed;
                _SoftLanding.State.Parameter = parameter;
            }
        }

        /************************************************************************************************************************/
    }
}
