using UniRx;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public float initialFuel = 10f;
    public float burnFuelPerSecond = 1f;
    private FloatReactiveProperty fuel;
    public IObservable<float> OnFuelAmountChanges() { return fuel; }
    private Subject<Unit> onCompletlyBurned;

    /// <summary>
    /// fires <b>OnCompleted</b> after fuel is set to 0
    /// </summary>
    public IObservable<Unit> OnCompletlyBurned()
    {
        return onCompletlyBurned ?? (onCompletlyBurned = new Subject<Unit>());
    }
    private bool isCompletlyBurned = false;

    void Awake()
    {
        fuel = new FloatReactiveProperty(initialFuel);
    }

    public float FuelAmount
    {
        get { return fuel.Value; }
        set {
            if (!isCompletlyBurned)
            {
                if (value <= 0f)
                {
                    fuel.Value = 0f;
                    isCompletlyBurned = true;
                    if (onCompletlyBurned != null)
                    {
                        onCompletlyBurned.OnCompleted();
                        fuel.Dispose();
                    }
                }
                else
                {
                    fuel.Value = value;
                }
            }
        }
    }
}
