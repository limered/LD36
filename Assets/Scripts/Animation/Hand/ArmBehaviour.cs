using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class ArmBehaviour : MonoBehaviour
    {
        public float RotationSpeed;

        private float _currentRotation;

        private void Update()
        {
            if(Input.GetMouseButton(0))
                Rotate();
        }

        private void Rotate()
        {
            var mouseX = Input.GetAxis("Mouse X") * RotationSpeed;

            _currentRotation = Mathf.Repeat(_currentRotation + mouseX, 360);

            var currRot = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(currRot.x, _currentRotation, currRot.z);
        }
    }
}
