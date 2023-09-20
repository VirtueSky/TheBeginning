using System;
using UnityEngine;
using VirtueSky.Core;

public class SoundManager : BaseMono
{
    public AudioSource backgroundAudio;
    public AudioSource fxAudio;

    [Header("Sound Generation")] [SerializeField]
    private AudioClip soundClickButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void Initialize()
    {
        base.Initialize();
        Setup();
    }

    public void OnMusicChanged()
    {
        backgroundAudio.mute = !Data.BgSoundState;
    }

    public void OnSoundChanged()
    {
        fxAudio.mute = !Data.FxSoundState;
    }

    public void Setup()
    {
        OnMusicChanged();
        OnSoundChanged();
    }

    public void PlaySoundFx(AudioClip _audioClip)
    {
        fxAudio.PlayOneShot(_audioClip);
    }

    public void PlayBackgroundMusic(AudioClip _audioClip)
    {
        backgroundAudio.clip = _audioClip;
        backgroundAudio.Play();
    }

    public void PauseBackground()
    {
        if (backgroundAudio)
        {
            backgroundAudio.Pause();
        }
    }

    public void ClickButton()
    {
        PlaySoundFx(soundClickButton);
    }
}