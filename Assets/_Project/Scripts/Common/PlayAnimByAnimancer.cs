using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class PlayAnimByAnimancer : MonoBehaviour
{
    public AnimancerComponent animancer;

    public void PlayAnim(ClipTransition clip, Action _endAnim = null, float _durationFade = .2f)
    {
        if (!animancer.IsPlaying(clip))
        {
            animancer.Play(clip, clip.Clip.length * _durationFade).Events.OnEnd = () => { _endAnim?.Invoke(); };
        }
    }
#if UNITY_EDITOR
    private void Reset()
    {
        if (animancer == null)
        {
            animancer = GetComponent<AnimancerComponent>();
        }
    }
#endif
}