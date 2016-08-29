using Assets.Scripts.Animation.Hand;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
    public class FingertipBehaviour : MonoBehaviour
    {
        #region Public Fields

        public GameObject FingerBehaviour;

        #endregion Public Fields

        #region Private Fields

        private GameObject _currentHoldObject;
        private FixedJoint _currentJoint;

        #endregion Private Fields

        #region Private Methods

        private void CollisionHandling(Collision collision)
        {
            if (!collision.contacts.Any()) return;

            GrabOntoObject(collision.gameObject);
        }

        private void GrabOntoObject(GameObject other)
        {
            if (!FingerBehaviour
                    .GetComponent<FingerBehaviour>()
                    .IsTighten.Value || _currentHoldObject) return;

            _currentHoldObject = other;

            var otherGrabbable = other.GetComponent<Grababble>();
            if (otherGrabbable == null) return;

            _currentJoint = (otherGrabbable.ObjectToLink == null)
                ? other.AddComponent<FixedJoint>()
                : otherGrabbable.ObjectToLink.AddComponent<FixedJoint>();

            _currentJoint.connectedBody = GetComponent<Rigidbody>();
        }

        private void LetGo()
        {
            Destroy(_currentJoint);
            _currentJoint = null;
            _currentHoldObject = null;
        }

        private void Start()
        {
            if (FingerBehaviour)
                FingerBehaviour
                    .GetComponent<FingerBehaviour>()
                    .IsTighten
                    .Skip(1)
                    .Where(b => b.Equals(false))
                    .Subscribe(b => LetGo())
                    .AddTo(this);

            this.OnCollisionEnterAsObservable()
                    .Subscribe(CollisionHandling)
                    .AddTo(this);
        }

        #endregion Private Methods
    }
}