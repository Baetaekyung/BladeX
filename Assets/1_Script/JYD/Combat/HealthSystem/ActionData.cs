using UnityEngine;



public struct ActionData
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public float damageAmount;
    public bool stun;
    
    public Vector3 knockbackDirection;
    public float knockbackForce;
    
    public int hurtType;
    public Color textColor;
    public ActionData( Vector3 hitPoint,  Vector3 hitNormal, float damageAmount, bool stun, Vector3 knockbackDirection = default, float knockbackForce = 0f, int hurtType = 0 ,Color textColor = default)
    {
        this.hitPoint = hitPoint;
        this.hitNormal = hitNormal;
        this.damageAmount = damageAmount;
        this.stun = stun;

        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
        this.hurtType = hurtType;
        this.textColor = textColor;
        
    }
    
}