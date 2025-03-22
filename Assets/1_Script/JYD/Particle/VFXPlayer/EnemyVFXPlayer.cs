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
            MonoGenericPool<ParrySign>.Initialize(unParrySignParticle);
        }

        public void PlayUnParrySign()
        {
            ParrySign parrySign = MonoGenericPool<ParrySign>.Pop();
            parrySign.transform.position = unParrySignParticleTrm.position;
            
        }
        
    }
}
