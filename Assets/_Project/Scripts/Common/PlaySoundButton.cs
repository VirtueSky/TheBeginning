using System;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;

public class PlaySoundButton : BaseMono
{
    [SerializeField] private ClickButtonEvent clickButtonEvent;
    [SerializeField] private EventAudioHandle playSoundButton;
    [SerializeField] private SoundData soundClickButton;

    private void Start()
    {
        clickButtonEvent.AddListener(OnClickButton);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        clickButtonEvent.RemoveListener(OnClickButton);
    }

    void OnClickButton()
    {
        playSoundButton.Raise(soundClickButton);
    }
}