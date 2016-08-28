using UniRx;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public float initialFuel = 10f;
    public float burnFuelPerSecond = 1f;
    public bool debugShowFuel = false;

    private FloatReactiveProperty fuel;
    public IObservable<float> OnFuelAmountChanges() { return fuel; }

    private bool isCompletlyBurned = false;

    void Awake()
    {
        fuel = new FloatReactiveProperty(initialFuel);
    }

    void Start()
    {
        if (debugShowFuel)
        {
            var textMesh = gameObject.AddHoverText(Color.yellow);
            fuel.Subscribe(f => textMesh.text = f.ToString("#") + " / " + initialFuel).AddTo(this);
        }
    }

    public float FuelAmount
    {
        get { return fuel.Value; }
        set
        {
            if (!isCompletlyBurned)
            {
                if (value <= 0f)
                {
                    fuel.Value = 0f;
                    isCompletlyBurned = true;
                    fuel.Dispose();
                }
                else
                {
                    fuel.Value = value;
                }
            }
        }
    }
}
