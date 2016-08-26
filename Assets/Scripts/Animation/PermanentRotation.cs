using UnityEngine;
using System.Collections;

public class PermanentRotation : MonoBehaviour
{
    private Vector3 lastRotation;
    public Vector3 EulerRotationSpeed;

    void Start()
    {
        lastRotation = gameObject.transform.eulerAngles;
    }

	void Update ()
	{
	    lastRotation = lastRotation + EulerRotationSpeed*Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(lastRotation);
	}
}
