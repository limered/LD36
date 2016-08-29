using UniRx;
using UnityEngine;

public class Durability : MonoBehaviour {

    public float initialHealth = 10f;
    public bool debugShowHealth = false;

    private FloatReactiveProperty health;
    public IObservable<float> OnHealthtChanges() { return health; }

    private bool isBroken = false;
    public bool IsBroken { get { return isBroken; } }

    void Awake()
    {
        health = new FloatReactiveProperty(initialHealth);

        if (debugShowHealth)
        {
            var textMesh = gameObject.AddHoverText(Color.white, 4f);
            health.Subscribe(f => textMesh.text = f.ToString("#.00") + " / " + initialHealth).AddTo(this);
        }
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
                    health.Dispose();
                }
                else
                {
                    health.Value = value;
                }
            }
        }
    }
}
