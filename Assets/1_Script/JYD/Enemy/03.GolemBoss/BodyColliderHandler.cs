using UnityEngine;

namespace Swift_Blade.Combat
{
    public class BodyColliderHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out IDamageble health))
                {
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 10,
                        knockbackDir = transform.forward,
                        knockbackDuration = 0.2f,
                        knockbackPower = 5,
                        dealer = transform
                    };
                    
                    health.TakeDamage(actionData);
                }
            }
        }
    }
}
