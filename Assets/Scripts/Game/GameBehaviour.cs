using UniRx;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameBehaviour : MonoBehaviour
    {
        //1
        public BoolReactiveProperty GrabbedSomething = new BoolReactiveProperty();

        //2
        public BoolReactiveProperty OneStoneSharpened = new BoolReactiveProperty();

        //3
        public BoolReactiveProperty ChoppedWood = new BoolReactiveProperty();

        //4
        public BoolReactiveProperty SteakCut = new BoolReactiveProperty();

        //5
        public IntReactiveProperty WoodBurning = new IntReactiveProperty();

        //6
        public BoolReactiveProperty Coocked = new BoolReactiveProperty();

        private AudioSource _audio;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _audio = GetComponent<AudioSource>();

            IoC.Resolve<Settings>()
                .OnMusicChanged
                .Subscribe(b => _audio.enabled = b)
                .AddTo(gameObject);
        }
    }
}