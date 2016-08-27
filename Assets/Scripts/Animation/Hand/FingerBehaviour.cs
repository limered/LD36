using UnityEngine;

namespace Assets.Scripts.Animation.Hand
{
    public class FingerBehaviour : MonoBehaviour
    {

        public GameObject TargetTight;
        public GameObject TargetLoose;
        public bool IsTighten;
        public KeyCode ButtonToPress;
    
        void Update ()
        {
            transform.position = ButtonToPress.IsPressed() ? TargetTight.transform.position : TargetLoose.transform.position;
        }
    }
}
