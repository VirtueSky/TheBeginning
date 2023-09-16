// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>
    /// A <see cref="CharacterState"/> which moves the character according to their
    /// <see cref="CharacterBrain.MovementDirection"/>.
    /// </summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/brains">Brains</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Brains/LocomotionState
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Brains - Locomotion State")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Brains) + "/" + nameof(LocomotionState))]
    public sealed class LocomotionState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private LinearMixerTransition _Animation;
        [SerializeField] private float _FadeSpeed = 2;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Character.Animancer.Play(_Animation);
            _Animation.State.Parameter = Character.Brain.IsRunning ? 1 : 0;
        }

        /************************************************************************************************************************/

        private void Update()
        {
            UpdateSpeed();
            UpdateTurning();
        }

        /************************************************************************************************************************/

        private void UpdateSpeed()
        {
            var target = Character.Brain.IsRunning ? 1 : 0;
            _Animation.State.Parameter = Mathf.MoveTowards(_Animation.State.Parameter, target, _FadeSpeed * Time.deltaTime);
        }

        /************************************************************************************************************************/

        private void UpdateTurning()
        {
            // Do not turn if we are not trying to move.
            var movement = Character.Brain.MovementDirection;
            if (movement == default)
                return;

            // Determine the angle we want to turn towards.
            // Without going into the maths behind it, Atan2 gives us the angle of a vector in radians.
            // So we just feed in the x and z values because we want an angle around the y axis,
            // then convert the result to degrees because Transform.eulerAngles uses degrees.
            var targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            // Determine how far we can turn this frame (in degrees).
            var turnDelta = Character.Stats.TurnSpeed * Time.deltaTime;

            // Get the current rotation, move its y value towards the target, and apply it back to the Transform.
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
    }
}
