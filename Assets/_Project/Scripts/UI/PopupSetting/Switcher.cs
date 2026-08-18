using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Tweening;
using VirtueSky.Variables;
using VirtueSky.Vibration;

public class Switcher : MonoBehaviour
{
    [Header("Datas")] public SwitchState switchState = SwitchState.Idle;
    public bool isOn;
    [Header("Components")] public SettingType settingType;
    public Sprite on;
    public Sprite off;
    public Image switchBar;
    public Transform offPos;
    public Transform onPos;
    public TextMeshProUGUI switchText;

    [Header("Config attribute")] [Range(0.1f, 3f)]
    public float timeSwitching = .5f;

    [ShowIf(nameof(settingType), SettingType.BackgroundMusic)] [SerializeField]
    private FloatVariable musicChangedVariable;

    [ShowIf(nameof(settingType), SettingType.SoundFx)] [SerializeField]
    private FloatVariable soundFxChangeVariable;


    private void SetupData()
    {
        switch (settingType)
        {
            case SettingType.BackgroundMusic:
                isOn = MusicChanged;
                break;
            case SettingType.SoundFx:
                isOn = SoundFxChanged;
                break;
            case SettingType.Vibration:
                isOn = VibrateChanged;
                break;
        }
    }

    private void SetupUI()
    {
        if (switchText) switchText.text = isOn ? "On" : "Off";
        if (isOn)
        {
            switchBar.sprite = on;
        }
        else
        {
            switchBar.sprite = off;
        }
    }

    private void Setup()
    {
        SetupData();
        SetupUI();
    }

    private void OnEnable()
    {
        Setup();
        switchBar.transform.position = isOn ? onPos.position : offPos.position;
    }

    public void Switching()
    {
        if (switchState == SwitchState.Moving) return;
        switchState = SwitchState.Moving;
        if (isOn)
        {
            Tween.Create(onPos.position, offPos.position, timeSwitching).BindToPosition(switchBar.transform);
        }
        else
        {
            Tween.Create(offPos.position, onPos.position, timeSwitching).BindToPosition(switchBar.transform);
        }

        Tween.Delay(timeSwitching / 2, () =>
        {
            switch (settingType)
            {
                case SettingType.BackgroundMusic:
                    MusicChanged = !isOn;
                    break;
                case SettingType.SoundFx:
                    SoundFxChanged = !isOn;
                    break;
                case SettingType.Vibration:
                    VibrateChanged = !isOn;
                    break;
            }

            Setup();
            switchState = SwitchState.Idle;
        });
    }

    private bool MusicChanged
    {
        get => musicChangedVariable.Value == 1;
        set => musicChangedVariable.Value = value ? 1 : 0;
    }

    private bool SoundFxChanged
    {
        get => soundFxChangeVariable.Value == 1;
        set => soundFxChangeVariable.Value = value ? 1 : 0;
    }

    private bool VibrateChanged
    {
        get => Vibration.EnableVibration;
        set => Vibration.EnableVibration = value;
    }
}

public enum SettingType
{
    BackgroundMusic,
    SoundFx,
    Vibration,
}

public enum SwitchState
{
    Idle,
    Moving,
}