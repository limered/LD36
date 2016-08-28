using UniRx;
using UnityEngine;

public class SharpableByFriction : MonoBehaviour
{
    [Tooltip("Replaces this gameobject with an instance of this prefab when the object got sharped. If no prefab is assigned the gameobject is destroyed.")]
    public GameObject replaceWhenSharpWith;

    [Range(0f, 10000f)]
    public float grindsUntilSharp = 5;
    private FloatReactiveProperty grindsLeft;
    private Vector3 lastFriction;
    private bool sharp;

    /// <summary>
    /// OnNext => grindsLeft changed
    /// OnCompleted => when object is sharp
    /// </summary>
    public IObservable<float> OnGrindsLeftChanged { get { return grindsLeft;} }

    void Start()
    {
        grindsLeft = new FloatReactiveProperty(grindsUntilSharp);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Grinder>())
        {
            var dot = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(lastFriction.normalized, collision.relativeVelocity.normalized)));
            grindsLeft.Value = Mathf.Clamp(grindsLeft.Value - Mathf.Clamp(collision.relativeVelocity.magnitude * (1f - dot), 0f, Mathf.Min(grindsLeft.Value, 1f)), 0, grindsUntilSharp);
            lastFriction = collision.relativeVelocity;

            if (grindsLeft.Value <= 0f)
            {
                grindsLeft.Dispose();
            }
        }
    }

    private void Replace()
    {
        Destroy(gameObject);

        if (replaceWhenSharpWith)
        {
            var nextState = Instantiate(replaceWhenSharpWith);
            nextState.transform.parent = transform.parent;
            nextState.transform.localPosition = transform.localPosition;
            nextState.transform.localRotation = transform.localRotation;

            var rigidbodyNext = nextState.GetComponent<Rigidbody>();
            var rigidbody = GetComponent<Rigidbody>();
            if (rigidbody && rigidbodyNext)
            {
                rigidbodyNext.velocity = rigidbody.velocity;
                rigidbodyNext.angularVelocity = rigidbody.angularVelocity;
            }
        }
    }
}
