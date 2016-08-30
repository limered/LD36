using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Fuel))]
public class InflameableByFriction : MonoBehaviour
{
    public float frictionHeatMultiplier = 1f;
    public float cooldownMultiplier = 3f;
    public float frictionHeatToStartBurning = 10f;

    [SerializeField]
    private float currentHeat = 0f;
    private Vector3 lastFriction;

    private readonly CompositeDisposable burningStatusDisposable = new CompositeDisposable();
    
    private void StartBurning()
    {
        if (!burningStatusDisposable.Any() && !GetComponent<IsBurning>())
        {
            Debug.Log("start burning (because of friction)");

            gameObject.AddComponent<IsBurning>()
                .OnDestroyAsObservable()
                .Subscribe(x => burningStatusDisposable.Clear())
                .AddTo(burningStatusDisposable);
        }
    }

    void Update()
    {
        //cool down
        if (!burningStatusDisposable.Any())
        {
            currentHeat = Mathf.Clamp(currentHeat - (Time.deltaTime*cooldownMultiplier), 0f, float.PositiveInfinity);
            if (currentHeat >= frictionHeatToStartBurning)
            {
                StartBurning();
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<InflameableByFriction>()) return;
        var dot = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(lastFriction.normalized, collision.relativeVelocity.normalized)));
        currentHeat += Mathf.Clamp(collision.relativeVelocity.magnitude * (1f-dot) * frictionHeatMultiplier, 0f, frictionHeatToStartBurning/20f);
        lastFriction = collision.relativeVelocity;
    }
}
