// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>
    /// Base class for the various states a <see cref="Brains.Character"/> can be in and actions they can perform.
    /// </summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/brains">Brains</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Brains/CharacterState
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Brains - Character State")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Brains) + "/" + nameof(CharacterState))]
    public abstract class CharacterState : StateBehaviour, IOwnedState<CharacterState>
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;

        /// <summary>The <see cref="Brains.Character"/> that owns this state.</summary>
        public Character Character
        {
            get => _Character;
            set
            {
                // If this was the active state in the previous character, force it back to Idle.
                if (_Character != null &&
                    _Character.StateMachine.CurrentState == this)
                    _Character.Idle.ForceEnterState();

                _Character = value;
            }
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            _Character = gameObject.GetComponentInParentOrChildren<Character>();
        }
#endif

        /************************************************************************************************************************/

        public StateMachine<CharacterState> OwnerStateMachine => _Character.StateMachine;

        /************************************************************************************************************************/
    }
}
