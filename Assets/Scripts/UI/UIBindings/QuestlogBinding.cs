using Assets.Scripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class QuestlogBinding : MonoBehaviour
{
    public Toggle Quest1Toggle;
    public Toggle Quest2Toggle;
    public Toggle Quest3Toggle;
    public Toggle Quest4Toggle;
    public Toggle Quest5Toggle;
    public Toggle Quest6Toggle;
    public AudioClip questCompletedSound;

    private Settings settings;
    private AudioSource audioSource;

    void Start()
    {
        settings = IoC.Resolve<Settings>();
        audioSource = GetComponent<AudioSource>();

        var game = GameObject.Find("DaGame").GetComponent<GameBehaviour>();
        game.GrabbedSomething.StartWith(false).Subscribe(x => Quest1Toggle.isOn = x).AddTo(this);
        game.OneStoneSharpened.StartWith(false).Subscribe(x => Quest2Toggle.isOn = x).AddTo(this);
        game.ChoppedWood.StartWith(false).Subscribe(x => Quest3Toggle.isOn = x).AddTo(this);
        game.SteakCut.StartWith(false).Subscribe(x => Quest4Toggle.isOn = x).AddTo(this);
        game.WoodBurning.Select(i => i>4).Where(x=>x).Take(1).StartWith(false).Subscribe(x => Quest5Toggle.isOn = x).AddTo(this);
        game.Coocked.StartWith(false).Subscribe(x => Quest6Toggle.isOn = x).AddTo(this);


        Quest1Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x =>
        {
            gameObject.SetActive(true);
            PlayQuestCompletedSound();
        }).AddTo(this);
        Quest2Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x => PlayQuestCompletedSound()).AddTo(this);
        Quest3Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x => PlayQuestCompletedSound()).AddTo(this);
        Quest4Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x => PlayQuestCompletedSound()).AddTo(this);
        Quest5Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x => PlayQuestCompletedSound()).AddTo(this);
        Quest6Toggle.OnValueChangedAsObservable().Where(x => x).Subscribe(x => PlayQuestCompletedSound()).AddTo(this);

        gameObject.SetActive(false);
    }

    private void PlayQuestCompletedSound()
    {
        if (audioSource && questCompletedSound)
        {
            audioSource.PlayOneShot(questCompletedSound);
        }
    }
}
