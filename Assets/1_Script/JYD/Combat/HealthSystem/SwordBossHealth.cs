using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade.Combat.Health
{
    public class SwordBossHealth : BossBaseHealth
    {
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ActionData action = new ActionData(Vector3.zero, 0.5f, 10f, 20f, transform, AttackType.Parry);
                TakeDamage(action);
            }
        }

        public override void TakeDamage(ActionData actionData)
        {
            if (isDead) return;
            
            currentHealth -= actionData.damageAmount;
            OnChangeHealthEvent?.Invoke(currentHealth/ maxHealth);

            if (currentHealth <= 0)
            {
                TriggerState(BossState.Dead);
                Dead();
                return;
            }

            OnHitEvent?.Invoke(actionData);

        }
    }
}