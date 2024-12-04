using UnityEngine;
using UnityEngine.Events;

public abstract class DamageCaster : MonoBehaviour
{
    public LayerMask targetLayer;

    public UnityEvent OnCastDamageEvent;
    public abstract bool CastDamage();
    //public abstract Vector3 GetStartPosition();

    
}