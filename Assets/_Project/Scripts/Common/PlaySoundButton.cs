using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Core;
using VirtueSky.Events;

public class PlaySoundButton : BaseMono
{
    [SerializeField] private ClickButtonEvent clickButtonEvent;
    [SerializeField] private EventAudioHandle playSoundButton;
    [SerializeField] private SoundData soundClickButton;

    public override void OnEnable()
    {
        base.OnEnable();
        clickButtonEvent.AddListener(OnClickButton);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        clickButtonEvent.RemoveListener(OnClickButton);
    }

    void OnClickButton()
    {
        playSoundButton.Raise(soundClickButton);
    }
}