using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundBehaviourOnCollide : MonoBehaviour
    {
        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();

            gameObject.OnCollisionEnterAsObservable()
                .Subscribe(OnNext)
                .AddTo(gameObject);
        }

        private void OnNext(Collision collision)
        {
            if(IoC.Resolve<Settings>().SoundEnabled)
                _audio.PlayOneShot(_audio.clip);
        }
    }
}