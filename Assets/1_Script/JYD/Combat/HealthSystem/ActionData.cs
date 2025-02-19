using UnityEngine;



public struct ActionData
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public float damageAmount;
    
    public Transform dealer;
    public bool stun;
    public ActionData( Vector3 hitPoint,  Vector3 hitNormal, float damageAmount, Transform dealer, bool stun)
    {
        this.hitPoint = hitPoint;
        this.hitNormal = hitNormal;
        this.damageAmount = damageAmount;
        this.dealer = dealer;
        this.stun = stun;
    }
    
}