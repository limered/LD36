using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts.Game.GoalBehaviours
{
    [RequireComponent(typeof(Fuel))]
    public class BurnedWoodBehaviour : MonoBehaviour
    {
        private GameBehaviour _game;
        private bool _isBurning;

        private void Start()
        {
            var g = GameObject.Find("DaGame");
            if (g) _game = g.gameObject.GetComponent<GameBehaviour>();

            GetComponent<Fuel>().OnFuelAmountChanges()
                .Skip(1)
                .Take(1)
                .Subscribe(f =>
                {
                    _game.WoodBurning.Value++;
                    _isBurning = true;
                })
                .AddTo(gameObject);

            this.OnDestroyAsObservable()
                .Subscribe(unit =>
                {
                    if (_isBurning) _game.WoodBurning.Value--;
                })
                .AddTo(gameObject);
        }
    }
}
