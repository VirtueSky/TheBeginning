using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;
using VirtueSky.Variables;
using VirtueSky.Vibration;

public class Switcher : MonoBehaviour
{
    [Header("Datas")] public SwitchState SwitchState = SwitchState.Idle;
    public bool IsOn;
    [Header("Components")] public SettingType SettingType;
    public Sprite On;
    public Sprite Off;
    public Image Switch;
    public Transform OffPos;
    public Transform OnPos;
    public TextMeshProUGUI SwitchText;

    [Header("Config attribute")] [Range(0.1f, 3f)]
    public float TimeSwitching = .5f;

    [ShowIf(nameof(SettingType), SettingType.BackgroundMusic)] [SerializeField]
    private FloatVariable musicChangedVariable;

    [ShowIf(nameof(SettingType), SettingType.SoundFx)] [SerializeField]
    private FloatVariable soundFxChangeVariable;


    private void SetupData()
    {
        switch (SettingType)
        {
            case SettingType.BackgroundMusic:
                IsOn = MusicChanged;
                break;
            case SettingType.SoundFx:
                IsOn = SoundFxChanged;
                break;
            case SettingType.Vibration:
                IsOn = VibrateChanged;
                break;
        }
    }

    private void SetupUI()
    {
        if (SwitchText) SwitchText.text = IsOn ? "On" : "Off";
        if (IsOn)
        {
            Switch.sprite = On;
        }
        else
        {
            Switch.sprite = Off;
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
        Switch.transform.position = IsOn ? OnPos.position : OffPos.position;
    }

    public void Switching()
    {
        if (SwitchState == SwitchState.Moving) return;
        SwitchState = SwitchState.Moving;
        if (IsOn)
        {
            Switch.transform.DOMove(OffPos.position, TimeSwitching);
        }
        else
        {
            Switch.transform.DOMove(OnPos.position, TimeSwitching);
        }

        DOTween.Sequence().AppendInterval(TimeSwitching / 2f).SetEase(Ease.Linear).AppendCallback(
            () =>
            {
                switch (SettingType)
                {
                    case SettingType.BackgroundMusic:
                        MusicChanged = !IsOn;
                        break;
                    case SettingType.SoundFx:
                        SoundFxChanged = !IsOn;
                        break;
                    case SettingType.Vibration:
                        VibrateChanged = !IsOn;
                        break;
                }

                Setup();
            }).OnComplete(() => { SwitchState = SwitchState.Idle; });
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