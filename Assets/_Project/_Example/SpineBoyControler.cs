using System;
using Spine.Unity;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Misc;

public class SpineBoyControler : MonoBehaviour
{
    public SkeletonAnimation spineBoy;
    public string idle;
    public string walk;
    public string run;
    public string shoot;
    public string portal;
    public string jump;
    public string death;
    public string hoverboard;
    public PlaySfxEvent playSfxEvent;
    public SoundData soundShoot;
    public SoundData soundJump;

    private void Start()
    {
        PlayPortal();
    }

    public void PlayPortal()
    {
        spineBoy.PlayOnly(portal).OnComplete(() => { PlayIdle(); });
    }

    public void PlayIdle()
    {
        spineBoy.PlayOnly(idle, true);
    }

    public void PlayWalk()
    {
        spineBoy.PlayOnly(walk, true);
    }

    public void PlayRun()
    {
        spineBoy.PlayOnly(run, true);
    }

    public void PlayShoot()
    {
        spineBoy.PlayOnly(shoot);
        playSfxEvent.Raise(soundShoot);
    }

    public void PlayAddShoot()
    {
        spineBoy.AddAnimation(1, shoot, false);
        playSfxEvent.Raise(soundShoot);
    }

    public void PlayJump()
    {
        spineBoy.PlayOnly(jump);
        playSfxEvent.Raise(soundJump);
    }

    public void PlayDeath()
    {
        spineBoy.PlayOnly(death);
    }

    public void PlayHoverBoard()
    {
        spineBoy.PlayOnly(hoverboard, true);
    }
}