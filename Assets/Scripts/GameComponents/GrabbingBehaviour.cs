using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Animation.Hand;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
    public class GrabbingBehaviour : MonoBehaviour
    {
        private int _isGrabbing;
        private List<FixedJoint> _compJoints = new List<FixedJoint>();

        private void Start()
        {
            SetupColliers();
            SetupFingers();
        }

        private void SetupColliers()
        {
            var colliders = GetComponentsInChildren<FingertipBehaviour>();
            foreach (var coll in colliders)
            {
                coll.OnCollisionEnterAsObservable()
                    .Where(c => c.gameObject.GetComponent<Grababble>() != null)
                    .Subscribe(CollisionHandling)
                    .AddTo(this);
            }
        }

        private void SetupFingers()
        {
            var fingers = GetComponentsInChildren<FingerBehaviour>();
            foreach (var finger in fingers)
            {
                finger
                    .IsTighten
                    .Skip(1)
                    .Subscribe(FingerIsTighten)
                    .AddTo(this);
            }
        }

        private void FingerIsTighten(bool b)
        {
            if (b)
                _isGrabbing++;
            else
                _isGrabbing--;

            if(_isGrabbing < 2) 
                LetGo();
        }

        private void CollisionHandling(Collision collision)
        {
            if (_isGrabbing < 2 || !collision.contacts.Any()) return;

            var thisGo = collision.contacts[0].thisCollider.gameObject;
            var otherGo = collision.contacts[0].otherCollider.gameObject;

            var comp = otherGo.AddComponent<FixedJoint>();
            comp.connectedBody = thisGo.GetComponent<Rigidbody>();

            _compJoints.Add(comp);
        }

        private void LetGo()
        {
            foreach (var joint in _compJoints)
            {
                Destroy(joint);
            }
            _compJoints.Clear();
        }
    }
}