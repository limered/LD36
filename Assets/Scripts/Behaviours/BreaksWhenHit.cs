using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BreaksWhenHit : MonoBehaviour
{
    [EnumFlags]
    public HitType takeDamageFrom;

    [Tooltip("If this is not assigned the Durability of this gameobject is used")]
    public Durability durability;

    [Tooltip("If this is not assigned, this gameobject will be destroyed on break")]
    public GameObject destroyTarget;

    public GameObject[] additionalPartsWithColliders;

    public GameObjectWithOffset[] breaksInto = new GameObjectWithOffset[1];

    private Tuple<Collision, HitObject>? lastCollision;

    private void Start()
    {
        durability = durability ? durability : GetComponent<Durability>();
        durability.OnHealthtChanges().Subscribe(f => { }, Break).AddTo(this);


        var onCollision = gameObject.OnCollisionEnterAsObservable();
        if (additionalPartsWithColliders != null && additionalPartsWithColliders.Any())
        {
            onCollision = onCollision.Merge(additionalPartsWithColliders.Select(go=>go.OnCollisionEnterAsObservable()).ToArray());
        }

        onCollision

        //create tuple of (Collision, HitObject)
        .Select(col => new Tuple<Collision, HitObject>(col, col.gameObject.GetComponent<HitObject>()))

        //only objects with HitObject-Component and HitType matches with takeDamageFrom field
        .Where(colHit => colHit.Item2 != null && (takeDamageFrom & colHit.Item2.type) != 0)

        //debug
        //.Select(x => { Debug.Log(x.Item1+"    ====>   "+x.Item1.relativeVelocity.magnitude); return x; })

        //only collisions that are strong enough
        .Where(colHit => colHit.Item1.relativeVelocity.magnitude >= colHit.Item2.minForceToDoDamage)

        .Subscribe(Hit).AddTo(this);
    }

    private void Hit(Tuple<Collision, HitObject> colHit)
    {
        lastCollision = colHit;
        durability.HealthAmount -= colHit.Item2.damageWhenThisHitsSomething;
    }

    private void Break()
    {
        destroyTarget = destroyTarget ? destroyTarget : gameObject;
        Destroy(destroyTarget);

        if (breaksInto != null && breaksInto.Any(x => x != null))
        {
            var rigidbody = GetComponent<Rigidbody>();
            foreach (var item in breaksInto.Where(x=>x != null).ToArray())
            {
                var nextState = Instantiate(item.Object);
                nextState.transform.parent = destroyTarget.transform.parent;
                nextState.transform.localPosition = destroyTarget.transform.localPosition;
                nextState.transform.localRotation = destroyTarget.transform.localRotation;
                nextState.transform.position += item.Offset;

                var rigidbodyNext = nextState.GetComponent<Rigidbody>();
                if (rigidbody && rigidbodyNext)
                {
                    rigidbodyNext.velocity = lastCollision.HasValue ? lastCollision.Value.Item1.relativeVelocity/8f + item.Offset : Vector3.zero;
                    rigidbodyNext.angularVelocity = rigidbody.angularVelocity;
                }
            }
        }
    }
}

[Serializable]
public class GameObjectWithOffset
{
    public GameObject Object;
    public Vector3 Offset;
}