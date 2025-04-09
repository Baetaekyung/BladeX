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
            MonoGenericPool<UnParryParticle>.Initialize(unParrySignParticle);
        }
        
        public void PlayUnParrySign()
        {
            UnParryParticle unParryParticle = MonoGenericPool<UnParryParticle>.Pop();
            unParryParticle.transform.position = unParrySignParticleTrm.position;
            
        }
        
    }
}
