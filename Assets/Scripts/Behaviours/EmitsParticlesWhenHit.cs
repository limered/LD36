using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Durability))]
public class EmitsParticlesWhenHit : MonoBehaviour
{
    public GameObject particlePrefab;
    public ParticleEmissionRange particleEmissionRange = new ParticleEmissionRange(0, 20);

    private GameObject particlesGameObject;
    private ParticleSystem particles;

    void Start()
    {
        particlesGameObject = Instantiate(particlePrefab, gameObject.transform) as GameObject;
        particlesGameObject.transform.localPosition = Vector3.zero;
        particles = particlesGameObject.GetComponent<ParticleSystem>();

        GetComponent<Durability>()
            .OnHealthtChanges()
            .Skip(1)
            .Select(h => (int)(Mathf.Clamp01(h) * (particleEmissionRange.max-particleEmissionRange.min) + particleEmissionRange.min))
            .Subscribe(particles.Emit).AddTo(this);
    }
}

[Serializable]
public struct ParticleEmissionRange
{
    public int min, max;

    public ParticleEmissionRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
} 
