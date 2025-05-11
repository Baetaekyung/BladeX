using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade.Combat
{
    public class BodyColliderHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                ActionData actionData = new ActionData
                {
                    damageAmount = 1
                    ,stun = true
                };
                
                health.TakeDamage(actionData);
            }
        }
    }
}
