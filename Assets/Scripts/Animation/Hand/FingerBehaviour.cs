using UniRx;
using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class FingerBehaviour : MonoBehaviour
    {

        public GameObject TargetTight;
        public GameObject TargetLoose;
        public BoolReactiveProperty IsTighten = new BoolReactiveProperty(false);
        public KeyCode ButtonToPress;
    
        void Update ()
        {
            IsTighten.Value = ButtonToPress.IsPressed();
            transform.position = IsTighten.Value ? TargetTight.transform.position : TargetLoose.transform.position;
        }
    }
}
