using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade.Level.Obstacle
{
    public class FireTrap : MonoBehaviour
    {
        private const float DURATION = 0.5f;
        private float nextFireTime;
        private void OnTriggerStay(Collider other)
        {
            if (Time.time > nextFireTime)
            {
                if (other.TryGetComponent(out BaseEntityHealth baseEntityHealth))
                {
                    nextFireTime = Time.time + DURATION;
                    baseEntityHealth.TakeDamage(new ActionData { damageAmount = 1, stun = false });
                }
            }
        }
    }
}
