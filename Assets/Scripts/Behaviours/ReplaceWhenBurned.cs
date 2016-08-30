using UniRx;
using UnityEngine;

[RequireComponent(typeof(Fuel))]
public class ReplaceWhenBurned : MonoBehaviour
{
    [Tooltip("Replaces this gameobject with an instance of this prefab. If no prefab is assigned the gameobject is destroyed.")]
    public GameObject replaceWith;
    public bool stillBurningAfterReplace = true;
    public GameObject destroyTarget;

    private Fuel fuel;

    void Start()
    {
        fuel = GetComponent<Fuel>();
        fuel.OnFuelAmountChanges().DoOnCompleted(Replace).Subscribe().AddTo(this);
    }

    private void Replace()
    {
        if(destroyTarget) Destroy(destroyTarget);
        else Destroy(gameObject);

        if (replaceWith)
        {
            var nextState = Instantiate(replaceWith);
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

            if (stillBurningAfterReplace)
            {
                var fuel = nextState.GetComponent<Fuel>();
                if (fuel)
                {
                    nextState.AddComponent<IsBurning>();
                }
            }
        }
    }
}
