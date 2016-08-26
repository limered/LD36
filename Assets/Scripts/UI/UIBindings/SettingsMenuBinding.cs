using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuBinding : MonoBehaviour
{
    public Toggle BoolFlagToggle;
    public Toggle ThisOrToggle;
    public Toggle ThatToggle;
    public Toggle SoundToggle;
    public Toggle MusicToggle;
    public Slider SensitivitySlider;
    public Text SensitivityText;
    public Button BackButton;
    private Settings settings;

    void Start()
    {
        settings = IoC.Resolve<Settings>();
        SetupBindings();
    }

    private void SetupBindings()
    {
        //Home Button
        BackButton.OnClickAsObservable().Subscribe(x=>IoC.Resolve<MenuNavigation>().NavigateToMainMenu()).AddTo(this);

        //BoolFlag toggle
        settings.OnBoolFlagChanged
            .Where(on => on != BoolFlagToggle.isOn)
            .Subscribe(on =>
            {
                BoolFlagToggle.isOn = on;
            }).AddTo(this);
        BoolFlagToggle.OnValueChangedAsObservable().Skip(1).Where(i=>i!=settings.BoolFlag).Subscribe(isOn => settings.BoolFlag = isOn).AddTo(this);

        //Sensitivity slider
        settings.OnSensitivityChanged.Subscribe(sens =>
        {
            SensitivityText.text = ((int)sens).ToString();
            if (!SensitivitySlider.value.Approx(sens)) SensitivitySlider.value = sens;
        }).AddTo(this);
        SensitivitySlider.OnValueChangedAsObservable().Skip(1).Subscribe(sens =>
        {
            settings.Sensitivity = sens;
        }).AddTo(this);

        //This Or This control
        settings.OnThisOrThatChanged.Subscribe(ct =>
        {
            ThatToggle.isOn = ct == Settings.ThisOrThat.That;
            ThisOrToggle.isOn = ct == Settings.ThisOrThat.This;
        }).AddTo(this);
        ThatToggle.OnValueChangedAsObservable().Where(x => x != (settings.ControllerType == Settings.ThisOrThat.That)).Subscribe(isOn => settings.ControllerType = Settings.ThisOrThat.That).AddTo(this);
        ThisOrToggle.OnValueChangedAsObservable().Where(x => x != (settings.ControllerType == Settings.ThisOrThat.This)).Subscribe(isOn => settings.ControllerType = Settings.ThisOrThat.This).AddTo(this);

        //Sound Toggle
        settings.OnSoundChanged.Subscribe(enabled =>
        {
            SoundToggle.isOn = enabled;
        }).AddTo(this);
        SoundToggle.OnValueChangedAsObservable().Subscribe(enabled => settings.SoundEnabled = enabled).AddTo(this);

        //Music Toggle
        settings.OnMusicChanged.Subscribe(enabled =>
        {
            MusicToggle.isOn = enabled;
        }).AddTo(this);
        MusicToggle.OnValueChangedAsObservable().Subscribe(enabled => settings.MusicEnabled = enabled).AddTo(this);
    }
}
