// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Characters
{
    /// <summary>A state for a <see cref="Character"/> which simply plays an animation.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/characters">Characters</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Characters/CharacterState
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Characters - Character State")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Characters) + "/" + nameof(CharacterState))]
    public class CharacterState : StateBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Character _Character;
        [SerializeField] private AnimationClip _Animation;

        /************************************************************************************************************************/

        /// <summary>
        /// Plays the animation and if it is not looping it returns the <see cref="Character"/> to Idle afterwards.
        /// </summary>
        private void OnEnable()
        {
            var state = _Character.Animancer.Play(_Animation, 0.25f);
            if (!_Animation.isLooping)
                state.Events.OnEnd = _Character.StateMachine.ForceSetDefaultState;
        }

        /************************************************************************************************************************/
    }
}
