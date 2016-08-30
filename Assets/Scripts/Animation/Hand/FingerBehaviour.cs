using UniRx;
using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class FingerBehaviour : MonoBehaviour
    {
        #region Public Fields

        public KeyCode Button;
        public BoolReactiveProperty IsTighten = new BoolReactiveProperty(false);
        public GameObject TargetLoose;
        public GameObject TargetTight;

        #endregion Public Fields

        #region Private Methods

        private void Start()
        {
            IsTighten
                .Skip(1)
                .Subscribe(
                    b =>
                        transform.position =
                            b.Equals(true) ? TargetTight.transform.position : TargetLoose.transform.position)
                .AddTo(this);
        }

        private void Update()
        {
            IsTighten.Value = Button.IsPressed();
        }

        #endregion Private Methods
    }
}