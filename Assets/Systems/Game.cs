using System;
using UnityEngine;

namespace Assets.Systems
{
    public class Game : MonoBehaviour, IGameSystem
    {
        public void Init()
        {

        }

        private void Start()
        {
            Init();
        }
        public void RegisterComponent(ILogicComponent component)
        {
            throw new NotImplementedException();
        }
    }
}