// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace Animancer.Examples.Layers
{
    /// <summary>Demonstrates how to use layers to play multiple animations at once on different body parts.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/layers">Layers</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.Layers/LayerExample
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Layers - Layer Example")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(Layers) + "/" + nameof(LayerExample))]
    public sealed class LayerExample : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _BasicAnimancer;
        [SerializeField] private AnimancerComponent _LayeredAnimancer;

        // Not using Transitions because the End Event is completely different between the two characters.
        [SerializeField] private AnimationClip _Idle;
        [SerializeField] private AnimationClip _Run;
        [SerializeField] private AnimationClip _Action;

        [SerializeField, Seconds] private float _FadeDuration = AnimancerPlayable.DefaultFadeDuration;
        [SerializeField] private AvatarMask _ActionMask;

        /************************************************************************************************************************/

        private const int BaseLayer = 0;
        private const int ActionLayer = 1;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // Idle on default layer 0.
            _BasicAnimancer.Play(_Idle);
            _LayeredAnimancer.Play(_Idle);

            // Set the mask for layer 1 (this automatically creates the layer).
            _LayeredAnimancer.Layers[ActionLayer].SetMask(_ActionMask);

            // Since we set a mask it will use the name of the mask in the Inspector by default. But we can also
            // replace it with a custom name. Either way, layer names are only used in the Inspector and any calls to
            // this method will be compiled out of runtime builds.
            _LayeredAnimancer.Layers[ActionLayer].SetDebugName("Action Layer");
        }

        /************************************************************************************************************************/

        private bool _IsRunning;

        public void ToggleRunning()
        {
            // Swap between true and false.
            _IsRunning = !_IsRunning;

            // Determine which animation to play.
            var animation = _IsRunning ? _Run : _Idle;

            // Play it on the basic character.
            _BasicAnimancer.Play(animation, _FadeDuration);

            // If the Action is currently playing on the layered character, move it to the appropriate layer.
            var previousLayer = _IsRunning ? BaseLayer : ActionLayer;
            var state = _LayeredAnimancer.Layers[previousLayer].CurrentState;

            if (state != null &&
                state.Weight > 0 &&
                state.Clip == _Action)
            {
                // Playing the animation on a different layer will need to create a copy of its state to let the
                // original fade out properly on the previous layer. So we need to give the new state the correct time.
                var time = state.Time;
                state = PerformActionLayered();
                state.Time = time;

                // If the Action is now on the base layer, let it continue instead of playing the Idle or Run.
                if (state.LayerIndex == BaseLayer)
                    return;
            }

            // Otherwise just play the Idle or Run animation normally.
            _LayeredAnimancer.Play(animation, _FadeDuration);
        }

        /************************************************************************************************************************/

        public void PerformAction()
        {
            // Basic.
            var state = _BasicAnimancer.Play(_Action, _FadeDuration);
            state.Events.OnEnd = () => _BasicAnimancer.Play(_IsRunning ? _Run : _Idle, _FadeDuration);

            // Layered.
            PerformActionLayered();
        }

        public AnimancerState PerformActionLayered()
        {
            // When running, perform the action on the ActionLayer (1) then fade that layer back out.
            if (_IsRunning)
            {
                var state = _LayeredAnimancer.Layers[ActionLayer].Play(_Action, _FadeDuration, FadeMode.FromStart);
                state.Events.OnEnd = () => state.Layer.StartFade(0, _FadeDuration);
                return state;
            }
            else// Otherwise perform the action on the BaseLayer (0) then return to idle.
            {
                var state = _LayeredAnimancer.Layers[BaseLayer].Play(_Action, _FadeDuration, FadeMode.FromStart);
                state.Events.OnEnd = () => _LayeredAnimancer.Play(_Idle, _FadeDuration);
                return state;
            }
        }

        /************************************************************************************************************************/
    }
}
