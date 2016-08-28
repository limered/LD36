using UniRx;
using UnityEngine;

[RequireComponent(typeof(Fuel))]
public class FuelLerpsScale : MonoBehaviour
{
    public Vector3 endScale = Vector3.one;
    public GameObject modelToScale;

    private Vector3 initialScale;
    private Fuel fuel;

    void Start()
    {
        initialScale = (modelToScale!=null ? modelToScale : gameObject).transform.localScale;

        fuel = GetComponent<Fuel>();
        fuel.OnFuelAmountChanges()
            .Subscribe(fuelAmount =>
            {
                (modelToScale != null ? modelToScale : gameObject).transform.localScale = Vector3.Lerp(initialScale, endScale, (fuel.initialFuel-fuelAmount) /fuel.initialFuel);
            },

            //on completed
            () => Destroy(this)
        ).AddTo(this);
    }
}
