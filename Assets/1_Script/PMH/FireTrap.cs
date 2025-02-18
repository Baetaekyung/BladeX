using UnityEngine;

namespace Swift_Blade
{
    public class FireTrap : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(new ActionData { damageAmount = 1, stun = false });
            }
        }
    }
}
