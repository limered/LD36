using System;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    public HitType type;
    public float damageWhenThisHitsSomething = 1f;
    public float minForceToDoDamage = 10f;
}

[Flags]
public enum HitType
{
    BluntObject = 1,
    SharpObject = 2,
}
