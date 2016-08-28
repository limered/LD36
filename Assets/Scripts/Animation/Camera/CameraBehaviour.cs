using UnityEngine;

namespace Assets.Scripts.Animation.Camera
{
    public class CameraBehaviour : MonoBehaviour
    {
        public GameObject Anchor;
        public float Speed;
        private Rigidbody _body;

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var anchorPos = Anchor.transform.position;

            var dir = (anchorPos - transform.position) * Speed;

            _body.AddForce(dir);
        }
    }
}