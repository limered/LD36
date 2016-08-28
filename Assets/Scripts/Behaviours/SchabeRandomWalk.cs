using UnityEngine;
using System.Collections;

public class SchabeRandomWalk : MonoBehaviour {

    public float secondsPerTarget = 2.0f;
    public Vector2 startPoint = new Vector2(0.0f, 0.0f);
    public Vector2 range = new Vector2(100.0f, 100.0f);
    public float maxSpeed = 10.0f;
    public float maxForce = 100.0f;

    private float targetTimeNoise;
    private Vector3 target;
    private Rigidbody rigidBody;


    // Use this for initialization
    void Awake ()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetTimeNoise = Random.Range(-1.0f, 1.0f);
        setTarget();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        if(!ableToWalk())
        {
            return;
        }

        float t = Time.fixedTime % (secondsPerTarget + targetTimeNoise);

        if (t < 0.01)
        {
            setTarget();

        }
        else
        {
            moveToTarget();
        }
    }

    public void setTarget()
    {
        setTarget(new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y)));
    }

    public void setTarget(Vector2 target)
    {
        this.target = new Vector3(target.x, transform.position.y, target.y) + new Vector3(startPoint.x, 0.0f, startPoint.y);

        Vector3 direction = (this.target - transform.position).normalized;

        float tmp = (Vector3.Dot(transform.rotation * -Vector3.right, direction) * 0.5f + 0.5f);

        if (tmp < 0.05f)
        {
            setTarget();
        }

    }

    public void moveToTarget()
    {
        Vector3 direction = (target - transform.position);
        float length = direction.magnitude;
        if(length < 1.0f)
        {
            setTarget();
            return;
        }
        else if(maxSpeed < rigidBody.velocity.magnitude)
        {
            return;
        }


        direction = direction / length;
        rigidBody.AddForce(direction * maxForce);
    }

    bool ableToWalk()
    {
        float result = Vector3.Dot(Vector3.up, transform.rotation * Vector3.up);

        return result > 0.75f;
    }
}
