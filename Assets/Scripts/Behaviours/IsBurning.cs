using System;
using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Fuel))]
public class IsBurning : MonoBehaviour
{
    public const string fireParticleSystemPrefabPath = "Prefabs/Particles/Fire_01";
    private static GameObject fireParticleCache;

    private Fuel fuel;
    private GameObject fireParticles;

    void Start()
    {
        fuel = GetComponent<Fuel>();

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
        fuel.fuel = Mathf.Clamp(fuel.fuel - Time.deltaTime*fuel.burnFuelPerSecond, 0f, float.PositiveInfinity);
        
        if (fuel.fuel <= 0f)
        {
            Destroy(fireParticles);
            Destroy(this);
        }
    }

    private static GameObject CachedFireParticle
    {
        get
        {
            var particles = fireParticleCache ??
                            (fireParticleCache = Resources.Load<GameObject>(fireParticleSystemPrefabPath));

            if (!particles) throw new Exception("failed to load resource "+fireParticleSystemPrefabPath);

            return particles;
        }
    }
}
