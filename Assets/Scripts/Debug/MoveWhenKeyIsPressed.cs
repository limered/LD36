using System.Linq;
using UnityEngine;

public class MoveWhenKeyIsPressed : MonoBehaviour
{
    public KeyCode[] keys = new KeyCode[0];
    public Vector3[] forces = new Vector3[0];

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].IsPressed())
            {
                body.AddForce(forces[i]);
            }
        }
    }
}