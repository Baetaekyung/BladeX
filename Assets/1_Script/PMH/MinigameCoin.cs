using UnityEngine;

namespace Swift_Blade
{
    public class MinigameCoin : MinigameItems
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                CoinManager.Instance.AddedCountCoin();
                Destroy(gameObject);
            }
        }
    }
}
