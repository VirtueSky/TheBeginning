// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Examples.StateMachines.Brains;
using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples.AnimatorControllers
{
    /// <summary>
    /// A <see cref="CharacterState"/> which allows the player to play golf using the
    /// <see cref="Events.GolfHitController"/> script.
    /// </summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/mini-game">Hybrid Mini Game</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers/GolfMiniGame
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Hybrid - Golf Mini Game")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(AnimatorControllers) + "/" + nameof(GolfMiniGame))]
    public sealed class GolfMiniGame : CharacterBrain
    {
        /************************************************************************************************************************/

        [SerializeField] private Events.GolfHitController _GolfHitController;
        [SerializeField] private Transform _GolfClub;
        [SerializeField] private Transform _ExitPoint;
        [SerializeField] private GameObject _RegularControls;
        [SerializeField] private GameObject _GolfControls;

        private Vector3 _GolfClubStartPosition;
        private Quaternion _GolfClubStartRotation;
        private CharacterBrain _PreviousBrain;

        private enum State { Entering, Turning, Playing, Exiting, }
        private State _State;

        /************************************************************************************************************************/

        private void Awake()
        {
            _GolfClubStartPosition = _GolfClub.localPosition;
            _GolfClubStartRotation = _GolfClub.localRotation;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// When a <see cref="Character"/> enters this trigger, try to make it enter this state.
        /// </summary>
        private void OnTriggerEnter(Collider collider)
        {
            if (enabled)
                return;

            var character = collider.GetComponent<Character>();
            if (character == null ||
                !character.Idle.TryEnterState())
                return;

            _State = State.Entering;
            _PreviousBrain = character.Brain;
            Character = character;
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            switch (_State)
            {
                case State.Entering:
                    if (MoveTowards(_GolfHitController.transform.position))
                        StartTurning();
                    break;

                case State.Turning:
                    if (Quaternion.Angle(Character.Animancer.transform.rotation, _GolfHitController.transform.rotation) < 1)
                        StartPlaying();
                    break;

                case State.Playing:
                    break;

                case State.Exiting:
                    if (MoveTowards(_ExitPoint.position))
                        Character.Brain = _PreviousBrain;
                    break;
            }
        }

        /************************************************************************************************************************/

        private bool MoveTowards(Vector3 destination)
        {
            var step = Character.Stats.GetMoveSpeed(false) * Time.deltaTime;
            var direction = destination - Character.Rigidbody.position;
            var distance = direction.magnitude;
            MovementDirection = direction / distance;// Normalize.
            return distance <= step;
        }

        /************************************************************************************************************************/

        private void StartTurning()
        {
            _State = State.Turning;
            MovementDirection = _GolfHitController.transform.forward;

            // Disable the Character's movement and move them next to the golf ball.
            Character.Rigidbody.velocity = default;
            Character.Rigidbody.isKinematic = true;
            Character.Rigidbody.position = _GolfHitController.transform.position;
        }

        /************************************************************************************************************************/

        private void StartPlaying()
        {
            _State = State.Playing;

            // Put the GolfClub in their hand, specifically as a child of the "RightHandHolder" object which is positioned
            // correctly for holding objects.
            const string HolderName = "RightHandHolder";
            var rightHand = Character.Animancer.Animator.GetBoneTransform(HumanBodyBones.RightHand);
            rightHand = rightHand.Find(HolderName);
            Debug.Assert(rightHand != null, "Unable to find " + HolderName);

            _GolfClub.parent = rightHand;
            _GolfClub.localPosition = default;
            _GolfClub.localRotation = Quaternion.identity;

            // Activate the GolfHitController so it can now take control of the character.
            _GolfHitController.gameObject.SetActive(true);

            // Swap the displayed controls.
            _RegularControls.SetActive(false);
            _GolfControls.SetActive(true);
        }

        /************************************************************************************************************************/

        public void Quit()
        {
            // Basically just undo everything StartTurning and StartPlaying did.

            _State = State.Exiting;

            _GolfHitController.gameObject.SetActive(false);
            _RegularControls.SetActive(true);
            _GolfControls.SetActive(false);

            _GolfClub.parent = transform;
            _GolfClub.localPosition = _GolfClubStartPosition;
            _GolfClub.localRotation = _GolfClubStartRotation;

            Character.Rigidbody.isKinematic = false;

            // The character will still be in the Idle state from when the mini game started.
            // But it only plays its animation when first entered so we force the character to re-enter that state.
            Character.Idle.TryReEnterState();
        }

        /************************************************************************************************************************/
    }
}
