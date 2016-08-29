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

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}