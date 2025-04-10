using UnityEngine;
using Swift_Blade.Combat.Caster;
using Swift_Blade.Pool;
using UnityEngine.AI;

namespace Swift_Blade.Enemy.Boss
{
    public class SorcererAnimationController : BaseEnemyAnimationController
    {
        [Header("Flameshrower")]
        [SerializeField] private SquareCaster[] _flameshrowerCasters;
        
        [Header("Suicide Bomb")]
        [SerializeField] private BaseEnemyCaster _suicideCaster;
        
        [Header("Fire Arrow")]
        [SerializeField] private FireArrow _fireArrowPrefab;
        [SerializeField] private Transform _fireArrowSpawnTrm;

        [Header("Explosion")]
        [SerializeField] private PoolPrefabMonoBehaviourSO _explosion;
        [SerializeField] private Transform _explosionSpawnTrm;
        [SerializeField] private BaseEnemyCaster _explosionCaster;

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

        private void CastFlameshrower(int index) => _flameshrowerCasters[index].Cast();
        private void SuicideBomb() => _suicideCaster.Cast();

        private void SpawnFireArrow()
        {
            Instantiate(_fireArrowPrefab, _fireArrowSpawnTrm.position, _fireArrowSpawnTrm.rotation);
        }

        private void SpawnExplosionEffect()
        {
            ExplosionParticle particle = MonoGenericPool<ExplosionParticle>.Pop();
            particle.transform.position = _explosionSpawnTrm.position;
            particle.transform.localScale = Vector3.one * 2.3f;

            _explosionCaster.Cast();
        }
    }
}
