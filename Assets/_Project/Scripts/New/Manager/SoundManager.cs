using System;
using UnityEngine;
using VirtueSky.Core;

public class SoundManager : BaseMono
{
    public AudioSource backgroundAudio;
    public AudioSource fxAudio;
    public SoundConfig SoundConfig => Config.Sound;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void Initialize()
    {
        base.Initialize();
        Setup();
        Observer.MusicChanged += OnMusicChanged;
        Observer.SoundChanged += OnSoundChanged;
    }

    private void OnMusicChanged()
    {
        backgroundAudio.mute = !Data.BgSoundState;
    }

    private void OnSoundChanged()
    {
        fxAudio.mute = !Data.FxSoundState;
    }

    public void Setup()
    {
        OnMusicChanged();
        OnSoundChanged();
    }

    public void PlayFX(SoundType soundType)
    {
        SoundData soundData = SoundConfig.GetSoundDataByType(soundType);

        if (soundData != null)
        {
            fxAudio.PlayOneShot(soundData.GetRandomAudioClip());
        }
        else
        {
            Debug.LogWarning("Can't found sound data");
        }
    }

    public void PlayBackground(SoundType soundType)
    {
        SoundData soundData = SoundConfig.GetSoundDataByType(soundType);

        if (soundData != null)
        {
            backgroundAudio.clip = soundData.GetRandomAudioClip();
            backgroundAudio.Play();
        }
        else
        {
            Debug.LogWarning("Can't found sound data");
        }
    }

    public void PauseBackground()
    {
        if (backgroundAudio)
        {
            backgroundAudio.Pause();
        }
    }

    #region ActionEvent

    // private void StartLevel(Level level)
    // {
    //     PlayFX(SoundType.StartLevel);
    // }
    //
    // private void WinLevel(Level level)
    // {
    //     PlayFX(SoundType.WinLevel);
    // }
    //
    // private void LoseLevel(Level level)
    // {
    //     PlayFX(SoundType.LoseLevel);
    // }
    //
    // private void ClickButton()
    // {
    //     PlayFX(SoundType.ClickButton);
    // }
    //
    // private void CoinMove()
    // {
    //     PlayFX(SoundType.CoinMove);
    // }
    //
    // private void PurchaseSucceed()
    // {
    //     PlayFX(SoundType.PurchaseSucceed);
    // }

    #endregion
}