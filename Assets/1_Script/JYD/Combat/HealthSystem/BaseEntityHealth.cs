using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Health
{
    public class BaseEntityHealth : MonoBehaviour , IHealth
    {
        public UnityEvent<ActionData> OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        [Header("Health info")]
        public float maxHealth;
        public bool isDead;
        
        public virtual void TakeDamage(ActionData actionData)
        {
        }
        
        public virtual void TakeHeal(float amount)
        {
        }

        public virtual void Dead()
        {
            isDead = true;
            OnDeadEvent?.Invoke();
        }
    }
}