using UnityEngine;

namespace Swift_Blade.Combat.Health
{
    public class GolemBossHealth : ThrowEnemyHealth
    {
        public override void TakeDamage(ActionData actionData)
        {
            if(isDead || !IsDamageTime())return;
            
            lastDamageTime = Time.time; 
            currentHealth -= actionData.damageAmount;
            
            OnHitEvent?.Invoke(actionData);
                        
            if (currentHealth <= 0)
            {
                TriggerState(BossState.Dead);
                Dead();
            }
        }
        
    }
                    
    
}
