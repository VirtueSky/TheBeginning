// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.AnimatorControllers.GameKit
{
    /// <summary>Base class for controlling the actions of a <see cref="Brains.Character"/>.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/3d-game-kit">3D Game Kit</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers.GameKit/CharacterBrain
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Game Kit - Character Brain")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(AnimatorControllers) + "." + nameof(GameKit) + "/" + nameof(CharacterBrain))]
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
                if (_Character == value)
                    return;

                var oldCharacter = _Character;
                _Character = value;

                // Make sure the old character doesn't still reference this brain.
                if (oldCharacter != null)
                    oldCharacter.Brain = null;

                // Give the new character a reference to this brain.
                if (value != null)
                    value.Brain = this;
            }
        }

        /************************************************************************************************************************/

        public Vector3 Movement { get; protected set; }

        /************************************************************************************************************************/
    }
}
