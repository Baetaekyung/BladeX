using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Caster
{
    public abstract class LayerCaster : MonoBehaviour, ICasterAble
    {
        public LayerMask targetLayer;
        public UnityEvent<ActionData> OnCastDamageEvent;
        public UnityEvent OnCastEvent;
        public abstract bool Cast();
        public abstract bool Cast(float additionalDamage = 0, float additionalCastingDistance = 0, bool stun = false);
    }
}