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
                    ActionData actionData = new ActionData(Vector3.zero,  Vector3.zero,  1, transform,true);
                    
                    health.TakeDamage(actionData);
                }
            }
        }
    }
}
