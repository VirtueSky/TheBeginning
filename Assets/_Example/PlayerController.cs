using System;
using Animancer;
using UnityEngine;
using VirtueSky.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayAnimByAnimancer playAnimByAnimancer;
    [SerializeField] private ClipTransition idle;
    [SerializeField] private ClipTransition run;
    [SerializeField] private ClipTransition walk;
    [SerializeField] private ClipTransition jump;
    [SerializeField] private ClipTransition dance;
    [SerializeField] private StringEvent playAnimEvent;

    private void OnEnable()
    {
        playAnimEvent.AddListener(PlayAnimCharacter);
    }

    private void OnDisable()
    {
        playAnimEvent.RemoveListener(PlayAnimCharacter);
    }

    public void PlayAnimCharacter(string animName)
    {
        playAnimByAnimancer.PlayAnim(GetClipTransition(animName));
    }

    public ClipTransition GetClipTransition(string animName)
    {
        switch (animName)
        {
            case "idle":
                return idle;
            case "run":
                return run;
            case "walk":
                return walk;
            case "jump":
                return jump;
            case "dance":
                return dance;
        }

        return idle;
    }
}