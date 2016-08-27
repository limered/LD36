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
    private float timeUnderFire = 0f;

    private readonly List<Collider> onFireBy = new List<Collider>();
    private int fireLayer;
    private Fuel fuel;
    private CompositeDisposable burningStatusDisposable = new CompositeDisposable();

    void Start()
    {
        fuel = GetComponent<Fuel>();
        fireLayer = LayerMask.NameToLayer("Fire");
    }

    private void StartBurning()
    {
        if (!burningStatusDisposable.Any())
        {
            Debug.Log("start burning");

            gameObject.AddComponent<IsBurning>()
                .OnDestroyAsObservable()
                .Subscribe(x => burningStatusDisposable.Clear())
                .AddTo(burningStatusDisposable);
        }
    }

    void Update()
    {
        if (onFireBy.Any())
        {
            timeUnderFire += Time.deltaTime;

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
