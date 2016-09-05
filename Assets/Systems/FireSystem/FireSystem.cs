using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Assets.Systems.FireSystem.Components;
using System;

namespace Assets.Systems.FireSystem
{
    public class FireSystem : MonoBehaviour, IGameSystem
    {
        public GameObject FireParticlePrefab;
        private Game _game;
        // private List<FuelComponent> _fuelComponents = new List<FuelComponent>();
        // private List<IsBurningComponent> _burningComponents = new List<IsBurningComponent>();
        public void Init()
        {
            // _game = IoC.Resolve<Game>();
        }

        public void RegisterComponent(ILogicComponent component)
        {
            if (component is FuelComponent)
            {
                // _fuelComponents.Add(component as FuelComponent);
                var fuelcomponent = component as FuelComponent;

                // var monoComponent = component as MonoBehaviour;
                // monoComponent
                //     .OnDestroyAsObservable()
                //     .Subscribe(c => _fuelComponents.Remove(component as FuelComponent))
                //     .AddTo(monoComponent);
            }
            else if (component is IsBurningComponent)
            {
                // _burningComponents.Add(component as IsBurningComponent);

                var monoComponent = component as MonoBehaviour;
                // monoComponent
                //     .OnDestroyAsObservable()
                //     .Subscribe(c => _burningComponents.Remove(component as IsBurningComponent))
                //     .AddTo(monoComponent);

                monoComponent
                    .UpdateAsObservable()
                    .Subscribe(c => Burn(monoComponent))
                    .AddTo(monoComponent);
            }
        }

        private void Burn(MonoBehaviour component)
        {
            var fuel = component.gameObject.GetComponent<FuelComponent>();
            fuel.Fuel.Value -= fuel.BurnFuelPerSecond * Time.deltaTime;
            if(fuel.Fuel.Value <= 0f)
                BurnedOut(component);
        }

        private void BurnedOut(MonoBehaviour component)
        {
            Destroy(component.gameObject);
        }
    }
}