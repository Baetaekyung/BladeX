using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class RollingRock : MinigameItems
    {
        private void OnTriggerEnter(Collider other)
        {
            PlayerHealth ph;
                print(other.gameObject);
            if(other.TryGetComponent<PlayerHealth>(out ph))
            {
                if(PlayerMinigameStatus.Instance.isCanBrokingrock)
                {
                    DestroyTheRock();
                }
                else
                {
                    ActionData ad = new ActionData();
                    ad.damageAmount = 1;
                    ph.TakeDamage(ad);
                } 
            }
        }

        public void DestroyTheRock()
        {
            Destroy(gameObject);
        }
    }
}
