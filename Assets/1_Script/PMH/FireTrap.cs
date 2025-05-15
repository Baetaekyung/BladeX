using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using UnityEngine;

namespace Swift_Blade.Level.Obstacle
{
    public class FireTrap : MonoBehaviour
    {
        private const float FIRE_DAMAGE = 0.4f;
        private const float FIRE_DAMAGE_DURATION = 2.4f;
        private const float DURATION = 0.5f;
        
        private float nextFireTime;
        private void OnTriggerStay(Collider other)
        {
            if (Time.time > nextFireTime)
            {
                if (other.TryGetComponent(out BaseEnemy enemy))
                {
                    nextFireTime = Time.time + DURATION;
                    enemy.GetEffectController().SetFire(FIRE_DAMAGE , FIRE_DAMAGE_DURATION);
                }
                else if (other.TryGetComponent(out PlayerHealth playerHealth))
                {
                    nextFireTime = Time.time + DURATION;
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 1,
                        stun = false
                    };
                    
                    playerHealth.TakeDamage(actionData);
                
                }
                
            }
        }
    }
}
