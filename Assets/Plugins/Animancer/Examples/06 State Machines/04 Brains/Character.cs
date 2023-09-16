// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using System;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>
    /// A centralised group of references to the common parts of a character and a state machine for their actions.
    /// </summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/brains">Brains</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Brains/Character
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Brains - Character")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Brains) + "/" + nameof(Character))]
    public sealed class Character : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private CharacterState _Idle;
        public CharacterState Idle => _Idle;

        [SerializeField]
        private Rigidbody _Rigidbody;
        public Rigidbody Rigidbody => _Rigidbody;

        [SerializeField]
        private CharacterBrain _Brain;
        public CharacterBrain Brain
        {
            get => _Brain;
            set
            {
                // The More Brains example uses this to swap between brains at runtime.

                if (_Brain == value)
                    return;

                var oldBrain = _Brain;
                _Brain = value;

                // Make sure the old brain doesn't still reference this character.
                if (oldBrain != null)
                    oldBrain.Character = null;

                // Give the new brain a reference to this character.
                if (value != null)
                    value.Character = this;
            }
        }

        [SerializeField]
        private CharacterStats _Stats;
        public CharacterStats Stats => _Stats;

        // Ground Detector.
        // Health and Mana.
        // Pathfinding.
        // Etc.
        // Anything common to most characters.

        /************************************************************************************************************************/

        /// <summary>The Finite State Machine that manages the actions of this character.</summary>
        public readonly StateMachine<CharacterState>.WithDefault
            StateMachine = new StateMachine<CharacterState>.WithDefault();

        private void Awake()
        {
            StateMachine.DefaultState = _Idle;
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Inspector Gadgets Pro calls this method after drawing the regular Inspector GUI, allowing this script to
        /// display its current state in Play Mode.
        /// </summary>
        /// <remarks>
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/pro">Inspector Gadgets Pro</see> allows you to
        /// easily customise the Inspector without writing a full custom Inspector class by simply adding a method with
        /// this name. Without Inspector Gadgets, this method will do nothing.
        /// </remarks>
        private void AfterInspectorGUI()
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                using (new UnityEditor.EditorGUI.DisabledScope(true))
                    UnityEditor.EditorGUILayout.ObjectField("Current State", StateMachine.CurrentState, typeof(CharacterState), true);
            }
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
