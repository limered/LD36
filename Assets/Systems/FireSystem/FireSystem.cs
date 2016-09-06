using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Assets.Systems.FireSystem.Components;

namespace Assets.Systems.FireSystem
{
    public class FireSystem : MonoBehaviour, IGameSystem
    {
        public GameObject FireParticlePrefab;
        private Game _game;
        private int _fireLayer;
        public void Init()
        {
            // _game = IoC.Resolve<Game>();
            _fireLayer = LayerMask.NameToLayer("Fire");
        }

        public void RegisterComponent(ILogicComponent component)
        {
            if (component is FuelComponent)
            {
                var fuelcomponent = component as FuelComponent;
                if (fuelcomponent.ByFire.IsActive)
                    SetUpInflamableByFire(fuelcomponent);
            }
            else if (component is IsBurningComponent)
            {
                SetUpBurning(component as MonoBehaviour);
            }
        }

        private void SetUpInflamableByFire(FuelComponent fuel)
        {
            fuel.OnTriggerStayAsObservable()
                .Where(collider => collider.gameObject.layer == _fireLayer)
                .Subscribe(collider => IsTouchingFire(collider, fuel))
                .AddTo(fuel);

            fuel.OnTriggerEnterAsObservable()
                .Where(collider => collider.gameObject.layer == _fireLayer)
                .Subscribe(collider => fuel.ByFire.OnFireBy.Add(collider))
                .AddTo(fuel);

            fuel.OnTriggerExitAsObservable()
                .Where(collider => collider.gameObject.layer == _fireLayer)
                .Subscribe(collider => fuel.ByFire.OnFireBy.Remove(collider))
                .AddTo(fuel);
        }

        private void IsTouchingFire(Collider other, FuelComponent current)
        {
            if (!current.ByFire.IsTouchingEnoughFire || current.gameObject.GetComponent<IsBurningComponent>()) return;
            
            current.ByFire.TimeUnderFire +=
                Time.deltaTime *
                (current.ByFire.FiresMultiply ? current.ByFire.OnFireBy.Count : 1);
            if (current.ByFire.TimeUnderFire >= current.ByFire.TimeUntilBurning)
                StartBurning(current);
        }

        private void StartBurning(FuelComponent fuel)
        {
            if (fuel.gameObject.GetComponent<IsBurningComponent>()) return;
            fuel.gameObject.AddComponent<IsBurningComponent>();
        }

//#region IsBurning
        private void SetUpBurning(MonoBehaviour component)
        {
            component
                .UpdateAsObservable()
                .Subscribe(c => Burn(component))
                .AddTo(component);

            AddFireParticles(component.gameObject);
        }

        private void AddFireParticles(GameObject gO){
            var fireParticles = Instantiate(FireParticlePrefab);
            fireParticles.transform.parent = gO.transform;
            
            fireParticles.transform.localScale = Vector3.one;
            fireParticles.transform.localPosition = Vector3.zero;
            fireParticles.transform.localRotation = Quaternion.identity;
            
            var fireBounds = gO.GetComponent<FireBounds>();
            if(fireBounds)
            {

            }
            else
            {

            }
        }

        private void Burn(MonoBehaviour component)
        {
            var fuel = component.gameObject.GetComponent<FuelComponent>();
            fuel.Fuel.Value -= fuel.BurnFuelPerSecond * Time.deltaTime;
            if (fuel.Fuel.Value <= 0f)
                BurnedOut(component);
        }
//#endregion IsBurning

        private void BurnedOut(MonoBehaviour component)
        {
            Destroy(component.gameObject);
        }
    }
}