using System;
using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class WristBehaviour : MonoBehaviour
    {
        public float MinValue;
        public float MaxValue;
        public float Speed;

        private float _currentRotation = 0;

        private void Start()
        {
            _currentRotation = (MinValue + MaxValue)*0.5f;
        }

        private void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            var mouseD = Input.GetAxis("Mouse Y");
            MoveWrist(-mouseD * Speed);
        }

        private void MoveWrist(float value)
        {
            _currentRotation = Mathf.Clamp(_currentRotation + value, MinValue, MaxValue);
            var currRot = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(currRot.x, currRot.y, _currentRotation);
        }
    }
}