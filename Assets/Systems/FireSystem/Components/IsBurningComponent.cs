using UnityEngine;
namespace Assets.Systems.FireSystem.Components
{
    public class IsBurningComponent : MonoBehaviour, ILogicComponent
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
    }
}