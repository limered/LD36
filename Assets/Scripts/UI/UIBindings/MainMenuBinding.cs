using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBinding : MonoBehaviour
{
    public Button StartButton;
    public Button EndButton;
    public Toggle SoundToggle;
    public Toggle MusicToggle;
    public AudioClip sfxToggleSound;

    private MenuNavigation menuNavigation;
    private Settings settings;
    private AudioSource audioSource;
    private bool firstStart = true;

    void Start()
    {
        menuNavigation = IoC.Resolve<MenuNavigation>();
        settings = IoC.Resolve<Settings>();

        audioSource = GetComponent<AudioSource>();

        //Start Button
        if (StartButton) StartButton.OnClickAsObservable().Subscribe(x => StartGame()).AddTo(this);

        //Start Button
        if (EndButton) EndButton.OnClickAsObservable().Subscribe(x => Application.Quit()).AddTo(this);

        //Sound Toggle
        settings.OnSoundChanged.Subscribe(on =>
        {
            if(SoundToggle) SoundToggle.isOn = on;
        }).AddTo(this);
        if (SoundToggle) SoundToggle.OnValueChangedAsObservable().Subscribe(on =>
        {
            settings.SoundEnabled = on;
            if(sfxToggleSound && on && !firstStart) audioSource.PlayOneShot(sfxToggleSound);
            if (firstStart) firstStart = false;
        }).AddTo(this);

        //Music Toggle
        settings.OnMusicChanged.Subscribe(on =>
        {
            if(MusicToggle) MusicToggle.isOn = on;
        }).AddTo(this);
        if (MusicToggle) MusicToggle.OnValueChangedAsObservable().Subscribe(on =>
        {
            settings.MusicEnabled = on;
            if(on && !audioSource.isPlaying) audioSource.Play();
            else if(!on) audioSource.Stop();
        }).AddTo(this);
    }

    private void StartGame()
    {
        menuNavigation.StartGame();
    }
}
