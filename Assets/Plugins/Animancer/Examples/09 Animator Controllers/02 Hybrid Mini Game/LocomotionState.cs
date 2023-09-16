// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

using Animancer.Examples.StateMachines.Brains;
using Animancer.Units;
using UnityEngine;

namespace Animancer.Examples.AnimatorControllers
{
    /// <summary>
    /// A <see cref="CharacterState"/> which moves the character according to their
    /// <see cref="CharacterBrain.MovementDirection"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// This class is very similar to <see cref="StateMachines.Brains.LocomotionState"/>, except that it manages a
    /// Blend Tree instead of individual <see cref="AnimationClip"/>s.
    /// </remarks>
    /// 
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/mini-game">Hybrid Mini Game</see></example>
    /// 
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers/LocomotionState
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Hybrid - Locomotion State")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(AnimatorControllers) + "/" + nameof(LocomotionState))]
    public sealed class LocomotionState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecondPerSecond] private float _Acceleration = 3;

        private float _MoveBlend;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Animancer.PlayController();
            _MoveBlend = 0;
        }

        /************************************************************************************************************************/

        private void Update()
        {
            UpdateAnimation();
            UpdateTurning();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// This method is similar to the "PlayMove" method in <see cref="Locomotion.IdleAndWalkAndRun"/>, but instead
        /// of checking <see cref="Input"/> to determine whether or not to run we are checking if the
        /// <see cref="Character.Brain"/> says it wants to run.
        /// </summary>
        private void UpdateAnimation()
        {
            float targetBlend;
            if (Character.Brain.MovementDirection == default)
                targetBlend = 0;
            else if (Character.Brain.IsRunning)
                targetBlend = 1;
            else
                targetBlend = 0.5f;

            _MoveBlend = Mathf.MoveTowards(_MoveBlend, targetBlend, _Acceleration * Time.deltaTime);
            Animancer.SetFloat("MoveBlend", _MoveBlend);
        }

        /************************************************************************************************************************/

        /// <remarks>
        /// This method is identical to the same method in <see cref="StateMachines.Brains.LocomotionState"/>.
        /// </remarks>
        private void UpdateTurning()
        {
            var movement = Character.Brain.MovementDirection;
            if (movement == default)
                return;

            var targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            var turnDelta = Character.Stats.TurnSpeed * Time.deltaTime;

            var transform = Character.Animancer.transform;
            var eulerAngles = transform.eulerAngles;
            eulerAngles.y = Mathf.MoveTowardsAngle(eulerAngles.y, targetAngle, turnDelta);
            transform.eulerAngles = eulerAngles;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constantly moves the character according to the <see cref="CharacterBrain.MovementDirection"/>.
        /// </summary>
        /// <remarks>
        /// This method is kept simple for the sake of demonstrating the animation system in this example.
        /// In a real game you would usually not set the velocity directly, but would instead use
        /// <see cref="Rigidbody.AddForce"/> to avoid interfering with collisions and other forces.
        /// <para></para>
        /// This method is identical to the same method in <see cref="StateMachines.Brains.LocomotionState"/>.
        /// </remarks>
        private void FixedUpdate()
        {
            // Get the desired direction, remove any vertical component, and limit the magnitude to 1 or less.
            // Otherwise a brain could make the character travel at any speed by setting a longer vector.
            var direction = Character.Brain.MovementDirection;
            direction.y = 0;
            direction = Vector3.ClampMagnitude(direction, 1);

            var speed = Character.Stats.GetMoveSpeed(Character.Brain.IsRunning);

            Character.Rigidbody.velocity = direction * speed;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Normally the <see cref="Character"/> class would have a reference to the specific type of
        /// <see cref="AnimancerComponent"/> we want, but for the sake of reusing code from the earlier example, we
        /// just use a type cast here.
        /// </summary>
        private HybridAnimancerComponent Animancer => (HybridAnimancerComponent)Character.Animancer;

        /************************************************************************************************************************/
    }
}
