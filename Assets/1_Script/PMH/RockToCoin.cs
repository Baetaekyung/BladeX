using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class RockToCoin : MinigameItems
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                BallGenerator.Instance.AllObjectToCoin();
                Destroy(gameObject);
            }
        }
    }
}
