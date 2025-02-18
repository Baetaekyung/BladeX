using UnityEngine;



public struct ActionData
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    /*public Vector3 knockbackDir;
    public float knockbackDuration;
    public float knockbackPower;*/
    
    public float damageAmount;
    public Transform dealer;
    public bool stun;
    public ActionData( Vector3 hitPoint,  Vector3 hitNormal,float damageAmount, Transform dealer, bool stun)
    {
        this.hitPoint = hitPoint;
        /*this.knockbackDir = knockbackDir;
        this.knockbackDuration = knockbackDuration;
        this.knockbackPower = knockbackPower;*/
        this.damageAmount = damageAmount;
        this.dealer = dealer;
        this.stun = stun;
        this.hitNormal = hitNormal;
    }
    
}