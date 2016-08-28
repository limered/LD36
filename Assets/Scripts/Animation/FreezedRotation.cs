using UnityEngine;

public class FreezedRotation : MonoBehaviour
{
    public Vector3 fixedRotation;

	
    void Update()
    {
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
