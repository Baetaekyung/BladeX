using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss
{
    public class SorcererAnimationController : BaseEnemyAnimationController
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO _explosion;
        [SerializeField] private Transform _explosionSpawnTrm;

        protected override void Start()
        {
            base.Start();
            
            MonoGenericPool<ExplosionParticle>.Initialize(_explosion);
        }

        private void SpawnExplosionEffect()
        {
            ExplosionParticle particle = MonoGenericPool<ExplosionParticle>.Pop();
            particle.transform.position = _explosionSpawnTrm.position;
        }
    }
}
