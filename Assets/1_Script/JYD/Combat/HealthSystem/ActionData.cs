using UnityEngine;

public enum AttackType
{
    Melee,Parry
}

public struct ActionData
{
    public Vector3 knockbackDir;
    public float knockbackDuration;
    public float knockbackPower;
        
    public float damageAmount;
    public Transform dealer;
    public AttackType attackType;
    
    public ActionData(Vector3 knockbackDir, float knockbackDuration, float knockbackPower, float damageAmount, Transform dealer, AttackType attackType)
    {
        this.knockbackDir = knockbackDir;
        this.knockbackDuration = knockbackDuration;
        this.knockbackPower = knockbackPower;
        this.damageAmount = damageAmount;
        this.dealer = dealer;
        this.attackType = attackType;
    }
    
}