using UniRx;
using UnityEngine;

public class Durability : MonoBehaviour {

    public float initialHealth = 10f;
    private FloatReactiveProperty health;
    public IObservable<float> OnHealthtChanges() { return health; }
    private Subject<Unit> onBreak;

    /// <summary>
    /// fires <b>OnCompleted</b> after health is set to 0
    /// </summary>
    public IObservable<Unit> OnBreak()
    {
        return onBreak ?? (onBreak = new Subject<Unit>());
    }
    private bool isBroken = false;
    public bool IsBroken { get { return isBroken; } }

    void Awake()
    {
        health = new FloatReactiveProperty(initialHealth);
    }

    public float HealthAmount
    {
        get { return health.Value; }
        set
        {
            if (!isBroken)
            {
                if (value <= 0f)
                {
                    health.Value = 0f;
                    isBroken = true;
                    if (onBreak != null)
                    {
                        onBreak.OnCompleted();
                        health.Dispose();
                    }
                }
                else
                {
                    health.Value = value;
                }
            }
        }
    }
}
