// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.Basics
{
    /// <summary>
    /// This script is basically the same as <see cref="PlayAnimationOnClick"/>, except that it uses
    /// <see href="https://kybernetik.com.au/animancer/docs/manual/transitions">Transitions</see>.
    /// </summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/basics/transitions">Transitions</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.Basics/PlayTransitionOnClick
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Basics - Play Transition On Click")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(Basics) + "/" + nameof(PlayTransitionOnClick))]
    public sealed class PlayTransitionOnClick : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private ClipTransition _Idle;
        [SerializeField] private ClipTransition _Action;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // Transitions store their events so we only initialize them once on startup
            // instead of setting the event every time the animation is played.
            _Action.Events.OnEnd = OnActionEnd;

            // The Fade Duration of this transition will be ignored because nothing else is playing yet so there is
            // nothing to fade from.
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _Animancer.Play(_Action);

                // The end event was already assigned in OnEnable.
            }
        }

        /************************************************************************************************************************/

        private void OnActionEnd()
        {
            // No need to hard-code the Fade Duration.
            // The transitions allow us to edit it in the Inspector.
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/
    }
}
