using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade.Level.Obstacle
{
    public class FireTrap : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out BaseEntityHealth baseEntityHealth))
            {
                baseEntityHealth.TakeDamage(new ActionData { damageAmount = 1, stun = false });
            }
        }
    }
}
