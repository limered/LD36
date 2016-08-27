using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public float MovementSpeed;

        private Rigidbody _body;

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_body == null || Input.GetMouseButton(0)) return;

            var mouseX = Input.GetAxis("Mouse X") * MovementSpeed * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * MovementSpeed * Time.deltaTime;

            var vertical = Input.GetMouseButton(1);

            var force = (vertical)
                ? new Vector3(0, mouseY, -mouseX)
                : new Vector3(mouseY, 0, -mouseX);

            _body.AddForce(force);
        }
    }
}