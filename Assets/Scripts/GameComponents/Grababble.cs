using UnityEngine;

namespace Assets.Scripts.GameComponents
{    
    [RequireComponent(typeof(Collider))]
    public class Grababble : MonoBehaviour
    {
        public GameObject ObjectToLink;
    }
}