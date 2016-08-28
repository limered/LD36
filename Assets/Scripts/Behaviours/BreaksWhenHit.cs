using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Durability))]
public class BreaksWhenHit : MonoBehaviour
{
    [InspectorExtensions.EnumFlags]
    public HitType takeDamageFrom;

    public GameObjectWithOffset[] breaksInto = new GameObjectWithOffset[1];

    private Durability durability;
    private Tuple<Collision, HitObject> lastCollision;

    private void Start()
    {
        durability = GetComponent<Durability>();
        durability.OnBreak().DoOnCompleted(Break).Subscribe().AddTo(this);

        gameObject.OnCollisionEnterAsObservable()

        //create tuple of (Collision, HitObject)
        .Select(col => new Tuple<Collision, HitObject>(col, col.gameObject.GetComponent<HitObject>()))

        //only objects with HitObject-Component and HitType matches with takeDamageFrom field
        .Where(colHit => colHit.Item2 != null && (takeDamageFrom & colHit.Item2.type) != 0)

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
        Destroy(gameObject);

        if (breaksInto != null && breaksInto.Any(x => x != null))
        {
            var rigidbody = GetComponent<Rigidbody>();
            foreach (var item in breaksInto.Where(x=>x != null).ToArray())
            {
                var nextState = Instantiate(item.Object);
                nextState.transform.parent = transform.parent;
                nextState.transform.localPosition = transform.localPosition;
                nextState.transform.localRotation = transform.localRotation;
                nextState.transform.position += item.Offset;

                var rigidbodyNext = nextState.GetComponent<Rigidbody>();
                if (rigidbody && rigidbodyNext)
                {
                    rigidbodyNext.velocity = lastCollision.Item1.relativeVelocity/8f + item.Offset;
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