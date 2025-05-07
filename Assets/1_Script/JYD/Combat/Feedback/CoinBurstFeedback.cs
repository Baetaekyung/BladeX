using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class CoinBurstFeedback : Feedback
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO coinBurst;
        [SerializeField] [Range(1,100)]private float random;
                
        private void Start()
        {
            MonoGenericPool<CoinBurstParticle>.Initialize(coinBurst);
        }

        public override void PlayFeedback()
        {
            if(CanSpawn())
                MonoGenericPool<CoinBurstParticle>.Pop().transform.position = transform.position + new Vector3(0,1,0);
        }

        public override void ResetFeedback()
        {
            
        }

        private bool CanSpawn()
        {
            return Random.Range(0, 100) < random;
        }
        
    }
}
