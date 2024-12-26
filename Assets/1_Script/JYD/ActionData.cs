using UnityEngine;

public enum AttackType
{
    VICINITY,
    RANGE
}

public struct ActionData
{
    public Vector3 knockbackDir;
    public float knockbackDuration;
    public float knockbackPower;

    public float damageAmount;
    public Transform dealer;

    public AttackType attackType;
}