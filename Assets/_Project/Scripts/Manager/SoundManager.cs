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

    public void StartLevel(Level level)
    {
        PlayFX(SoundType.StartLevel);
    }

    public void WinLevel(Level level)
    {
        PlayFX(SoundType.WinLevel);
    }

    public void LoseLevel(Level level)
    {
        PlayFX(SoundType.LoseLevel);
    }

    public void ClickButton()
    {
        PlayFX(SoundType.ClickButton);
    }

    public void CoinMove()
    {
        PlayFX(SoundType.CoinMove);
    }

    public void PurchaseSucceed()
    {
        PlayFX(SoundType.PurchaseSucceed);
    }

    #endregion
}