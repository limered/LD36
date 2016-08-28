using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Fuel))]
public class InflameableByFire : MonoBehaviour
{
    public float timeUntilStartsBurning = 3f;
    [Range(1, 100)]
    public int minFireCount = 1;
    public bool firesMultiply = false;
    private float timeUnderFire = 0f;

    private readonly List<Collider> onFireBy = new List<Collider>();
    private int fireLayer;
    private readonly CompositeDisposable burningStatusDisposable = new CompositeDisposable();

    void Start()
    {
        fireLayer = LayerMask.NameToLayer("Fire");
    }

    private void StartBurning()
    {
        if (!burningStatusDisposable.Any() && !GetComponent<IsBurning>())
        {
            gameObject.AddComponent<IsBurning>()
                .OnDestroyAsObservable()
                .Subscribe(x => burningStatusDisposable.Clear())
                .AddTo(burningStatusDisposable);
        }
    }

    void Update()
    {
        if (onFireBy.Count >= minFireCount)
        {
            timeUnderFire += Time.deltaTime * (firesMultiply ? onFireBy.Count : 1);

            if (timeUnderFire >= timeUntilStartsBurning)
            {
                StartBurning();
            }
        }
        else
        {
            timeUnderFire = 0f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == fireLayer && !onFireBy.Contains(other))
        {
            onFireBy.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == fireLayer)
        {
            onFireBy.Remove(other);
        }
    }
}
