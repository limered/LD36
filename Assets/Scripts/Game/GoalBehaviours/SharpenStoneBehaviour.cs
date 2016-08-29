using UnityEngine;

namespace Assets.Scripts.Game.GoalBehaviours
{
    public class SharpenStoneBehaviour : MonoBehaviour
    {
        private GameBehaviour _game;

        private void Start()
        {
            var g = GameObject.Find("DaGame");
            if (g) _game = g.gameObject.GetComponent<GameBehaviour>();

            _game.OneStoneSharpened.Value = true;
        }
    }
}