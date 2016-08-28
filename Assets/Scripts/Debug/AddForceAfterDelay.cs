using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody))]
public class AddForceAfterDelay : MonoBehaviour
{
    public Vector3 direction;
    public float force;
    public float delay;

    void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(delay))
            .DoOnCompleted(
                () =>
                {
                    GetComponent<Rigidbody>().AddForce(force * direction.normalized);
                    Destroy(this);
                }
            )
            .Subscribe().AddTo(this);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
    }
}
