using System;
using UniRx;
using UnityEngine;

public class SharpableByFriction : MonoBehaviour
{
    public const string sparksParticlesPrefabPath = "Prefabs/Particles/Sparks_01";
    private static GameObject sparksParticleCache;

    [Tooltip("Replaces this gameobject with an instance of this prefab when the object got sharped. If no prefab is assigned the gameobject is destroyed.")]
    public GameObject replaceWhenSharpWith;

    [Range(0f, 10000f)]
    public float grindsUntilSharp = 5;

    public bool emitSparks = true;

    [Range(1, 100)]
    public int maxSparks = 30;

    public bool debugShowGrindsUntilSharp;

    private FloatReactiveProperty grindsLeft;
    private Vector3 lastFriction;
    private bool sharp;
    private GameObject sparkParticlesGameObject;
    private ParticleSystem sparkParticles;

    /// <summary>
    /// OnNext => grindsLeft changed
    /// OnCompleted => when object is sharp
    /// </summary>
    public IObservable<float> OnGrindsLeftChanged { get { return grindsLeft; } }

    void Start()
    {
        grindsLeft = new FloatReactiveProperty(grindsUntilSharp); 

        if (emitSparks)
        {
            sparkParticlesGameObject = Instantiate(CachedSparksParticle, transform) as GameObject;
            sparkParticlesGameObject.transform.localScale = Vector3.one;
            sparkParticlesGameObject.transform.localPosition = Vector3.zero;
            sparkParticlesGameObject.transform.localRotation = Quaternion.identity;
            sparkParticles = sparkParticlesGameObject.GetComponent<ParticleSystem>();
        }

        if (debugShowGrindsUntilSharp)
        {
            var textMesh = gameObject.AddHoverText(Color.blue, -2f);
            grindsLeft.Subscribe(f => textMesh.text = f.ToString("#.00") + " / " + grindsUntilSharp).AddTo(this);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Grinder>())
        {
            var dot = Mathf.Clamp01(Mathf.Abs(Vector3.Dot(lastFriction.normalized, collision.relativeVelocity.normalized)));
            grindsLeft.Value = Mathf.Clamp(grindsLeft.Value - Mathf.Clamp(collision.relativeVelocity.magnitude * (1f - dot), 0f, Mathf.Min(grindsLeft.Value, 1f)), 0, grindsUntilSharp);
            lastFriction = collision.relativeVelocity;

            if (sparkParticlesGameObject)
            {
                var sparkCount = (int)(dot * maxSparks + 0.5f);
                sparkParticles.Emit(sparkCount);
            }

            if (grindsLeft.Value <= 0f)
            {
                grindsLeft.Dispose();
                Replace();
            }
        }
    }

    private void Replace()
    {
        Destroy(gameObject);

        if (replaceWhenSharpWith)
        {
            var nextState = Instantiate(replaceWhenSharpWith);
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
        }
    }

    private static GameObject CachedSparksParticle
    {
        get
        {
            var particles = sparksParticleCache ??
                            (sparksParticleCache = Resources.Load<GameObject>(sparksParticlesPrefabPath));

            if (!particles) throw new Exception("failed to load resource " + sparksParticlesPrefabPath);

            return particles;
        }
    }
}
