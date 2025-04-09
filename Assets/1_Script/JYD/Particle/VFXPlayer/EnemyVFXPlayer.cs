using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class EnemyVFXPlayer : MonoBehaviour
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO unParrySignParticle;
        [SerializeField] private Transform unParrySignParticleTrm;
        
        
        private void Start()
        {
            MonoGenericPool<ParrySignParticle>.Initialize(unParrySignParticle);
        }
        
        public void PlayUnParrySign()
        {
            ParrySignParticle parrySignParticle = MonoGenericPool<ParrySignParticle>.Pop();
            parrySignParticle.transform.position = unParrySignParticleTrm.position;
            
        }
        
    }
}
