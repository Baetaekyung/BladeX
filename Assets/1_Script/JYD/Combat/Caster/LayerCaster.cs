using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Caster
{
    public abstract class LayerCaster : MonoBehaviour
    {
        public LayerMask targetLayer;
        public UnityEvent OnCastDamageEvent;
        public abstract bool CastDamage();

    }
}