// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>Base class for controlling the actions of a <see cref="Brains.Character"/>.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/brains">Brains</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Brains/CharacterBrain
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Brains - Character Brain")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Brains) + "/" + nameof(CharacterBrain))]
    public abstract class CharacterBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;
        public Character Character
        {
            get => _Character;
            set
            {
                // The More Brains example uses this to swap between brains at runtime.

                if (_Character == value)
                    return;

                var oldCharacter = _Character;
                _Character = value;

                // Make sure the old character doesn't still reference this brain.
                if (oldCharacter != null)
                    oldCharacter.Brain = null;

                // Give the new character a reference to this brain.
                // We also only want brains to be enabled when they actually have a character to control.
                if (value != null)
                {
                    value.Brain = this;
                    enabled = true;
                }
                else
                {
                    enabled = false;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>The direction this brain wants to move.</summary>
        public Vector3 MovementDirection { get; protected set; }

        /// <summary>Indicates whether this brain wants to run.</summary>
        public bool IsRunning { get; protected set; }

        /************************************************************************************************************************/
    }
}
