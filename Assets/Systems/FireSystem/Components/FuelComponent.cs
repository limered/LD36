using UnityEngine;
using UniRx;
using System.Collections.Generic;

namespace Assets.Systems.FireSystem.Components
{
    public class FuelComponent : MonoBehaviour, ILogicComponent
    {
        private void Start()
        {
            RegisterToSystem();
        }

        //temporary, need better solution for this
        public void RegisterToSystem()
        {
            IoC.Resolve<FireSystem>()
                .RegisterComponent(this);
        }

        public FloatReactiveProperty Fuel = new FloatReactiveProperty();
        public float BurnFuelPerSecond = 1f;
        public bool DebugShowFuel = false;
        public InflamableByFire ByFire;
        public InflamableByFriction ByFriction;

        [System.Serializable]
        public class InflamableByFire
        {
            public bool IsActive;
            public float TimeUntilBurning;
            [Range(1, 10)]
            public int MinFireCount = 1;
            public bool FiresMultiply;

            public float TimeUnderFire { get; set; }
            private List<Collider> _onFireBy = new List<Collider>();
            public List<Collider> OnFireBy { get { return _onFireBy; } }
            public bool IsTouchingEnoughFire { get { return OnFireBy.Count >= MinFireCount; } }
        }

        [System.Serializable]
        public class InflamableByFriction
        {
            public bool IsActive;
            public float FrictionHeatMultiplier;
            public float CooldownMultiplier;
            public float FrictionHeatToStartBurning;
            public float CurrentHeat { get; set; }
            public Vector3 LastFriction { get; set; }
            private CompositeDisposable _burningStatusDisposable = new CompositeDisposable();
            public CompositeDisposable BurningStatusDisposable { get { return _burningStatusDisposable; } }
        }
    }
}