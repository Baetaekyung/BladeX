using UnityEngine;
using UnityEngine.Events;

public abstract class LayerCaster : MonoBehaviour
{
    public LayerMask targetLayer;
    public UnityEvent OnCastDamageEvent;
    public abstract bool CastDamage();
   
}