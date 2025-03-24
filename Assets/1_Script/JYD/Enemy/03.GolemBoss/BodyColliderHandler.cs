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
                    ActionData actionData = new ActionData();
                    actionData.damageAmount = 1;
                    
                    health.TakeDamage(actionData);
                }
            }
        }
    }
}
