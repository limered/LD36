using Assets.Scripts.GameComponents;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Game.GoalBehaviours
{
    public class GrabbedSomethingBehaviour : MonoBehaviour
    {
        private GameBehaviour _game;

        private void Start()
        {
            var g = GameObject.Find("DaGame");
            if (g) _game = g.gameObject.GetComponent<GameBehaviour>();

            var fingerTips = GetComponentsInChildren<FingertipBehaviour>();
            foreach (var tip in fingerTips)
            {
                tip.HasObject
                    .Where(x => x)
                    .Subscribe(x => _game.GrabbedSomething.Value = x)
                    .AddTo(this);
            }
        }
    }
}