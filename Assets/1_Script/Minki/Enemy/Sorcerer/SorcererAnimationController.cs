using UnityEngine;
using Swift_Blade.Combat.Caster;
using Swift_Blade.Pool;
using UnityEngine.AI;

namespace Swift_Blade.Enemy.Boss
{
    public class SorcererAnimationController : BaseEnemyAnimationController
    {
        [Header("Explosion")]
        [SerializeField] private PoolPrefabMonoBehaviourSO _explosion;
        [SerializeField] private Transform _explosionSpawnTrm;
        [SerializeField] private BaseEnemyCaster _explosionCaster;

        [Header("Flameshrower")]
        [SerializeField] private SquareCaster[] _flameshrowerCasters;

        protected override void Awake()
        {
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponentInParent<NavMeshAgent>();
        }

        protected override void Start()
        {
            base.Start();
            
            MonoGenericPool<ExplosionParticle>.Initialize(_explosion);
        }

        private void ResetLocalPosition() => transform.localPosition = Vector3.zero;

        private void SpawnExplosionEffect()
        {
            ExplosionParticle particle = MonoGenericPool<ExplosionParticle>.Pop();
            particle.transform.position = _explosionSpawnTrm.position;
            particle.transform.localScale = Vector3.one * 2.3f;

            _explosionCaster.Cast();
        }

        private void CastFlameshrower(int index)
        {
            _flameshrowerCasters[index].Cast();
        }
    }
}
