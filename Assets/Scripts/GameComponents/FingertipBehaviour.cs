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

        public ReactiveProperty<bool> HasObject = new ReactiveProperty<bool>(false);

        #endregion Public Fields

        #region Private Fields

        private readonly ReactiveProperty<GameObject> _currentHoldObject = new ReactiveProperty<GameObject>();
        private FixedJoint _currentJoint;
        private Collider _collider;

        #endregion Private Fields

        #region Private Methods

        private void CollisionHandling(Collision collision)
        {
            if (!collision.contacts.Any()) return;

            GrabOntoObject(collision.contacts[0].otherCollider.gameObject);
        }

        private void GrabOntoObject(GameObject other)
        {
            if (!FingerBehaviour
                    .GetComponent<FingerBehaviour>()
                    .IsTighten.Value || _currentHoldObject.Value) return;

            _currentHoldObject.Value = other;
            other.OnDestroyAsObservable()
                .Subscribe(unit => _currentHoldObject.Value = null)
                .AddTo(other);

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
            _currentHoldObject.Value = null;
        }

        private void Start()
        {
            _collider = GetComponent<Collider>();

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

            _currentHoldObject
                .Subscribe(o => HasObject.Value = o != null)
                .AddTo(this);

            HasObject
                .Subscribe(b => _collider.enabled = !b)
                .AddTo(this);
        }

        #endregion Private Methods
    }
}