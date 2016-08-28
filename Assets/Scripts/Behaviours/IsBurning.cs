using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Fuel))]
public class IsBurning : MonoBehaviour
{
    public const string fireParticlesPrefabPath = "Prefabs/Particles/Fire_01";
    private static GameObject fireParticleCache;

    private Fuel fuel;
    private GameObject fireParticles;

    void Start()
    {
        fuel = GetComponent<Fuel>();
        fuel.OnCompletlyBurned().DoOnCompleted(Extinct).Subscribe().AddTo(this);

        fireParticles = Instantiate(CachedFireParticle, transform) as GameObject;
        fireParticles.transform.localScale = Vector3.one;
        fireParticles.transform.localPosition = Vector3.zero;
        fireParticles.transform.localRotation = Quaternion.identity;

        var fireHitbox = fireParticles.GetComponentInChildren<BoxCollider>();
        var bounds = new Bounds(transform.position, Vector3.zero);
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            if(!(r is ParticleSystemRenderer)) bounds.Encapsulate(r.bounds);
        }

        fireHitbox.size = bounds.extents * 1.1f;
    }

    void Update()
    {
        fuel.FuelAmount = Mathf.Clamp(fuel.FuelAmount - Time.deltaTime*fuel.burnFuelPerSecond, 0f, float.PositiveInfinity);
    }

    private void Extinct()
    {
        Destroy(fireParticles);
        Destroy(this);
    }

    private static GameObject CachedFireParticle
    {
        get
        {
            var particles = fireParticleCache ??
                            (fireParticleCache = Resources.Load<GameObject>(fireParticlesPrefabPath));

            if (!particles) throw new Exception("failed to load resource "+fireParticlesPrefabPath);

            return particles;
        }
    }
}
