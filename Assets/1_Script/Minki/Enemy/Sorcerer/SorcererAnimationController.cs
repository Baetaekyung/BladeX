using Swift_Blade.Audio;
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

        [Header("Close Explosion")]
        [SerializeField] private PoolPrefabMonoBehaviourSO _closeExplosion;
        [SerializeField] private Transform _closeExplosionSpawnTrm;
        [SerializeField] private BaseEnemyCaster _closeExplosionCaster;
        
        [Header("Fire Arrow")]
        [SerializeField] private FireArrow _fireArrowPrefab;
        [SerializeField] private Transform _fireArrowSpawnTrm;

        [Header("Fire Projectile")]
        [SerializeField] private PoolPrefabMonoBehaviourSO _fireProjectile;
        [SerializeField] private Transform _fireProjectileSpawnTrm;

        [Header("Explosion")]
        [SerializeField] private PoolPrefabMonoBehaviourSO _explosion;
        [SerializeField] private Transform _explosionSpawnTrm;
        [SerializeField] private BaseEnemyCaster _explosionCaster;

        //[SerializeField] private AudioCollectionSO audioCollectionSo;
        
        protected override void Awake()
        {
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponentInParent<NavMeshAgent>();
        }
        
        protected override void Start()
        {
            base.Start();
            
            MonoGenericPool<CloseExplosionParticle>.Initialize(_closeExplosion);
            MonoGenericPool<FireProjectile>.Initialize(_fireProjectile);
            MonoGenericPool<ExplosionParticle>.Initialize(_explosion);
        }

        private void CastFlameshrower(int index) => _flameshrowerCasters[index].Cast();
        private void SuicideBomb() => _suicideCaster.Cast();

        private void SpawnCloseExplosionEffect()
        {
            ExplosionParticle particle = MonoGenericPool<ExplosionParticle>.Pop();
            particle.transform.position = _closeExplosionSpawnTrm.position;
            particle.transform.localScale = Vector3.one * 1.3f;

            _closeExplosionCaster.Cast();
        }

        private void SpawnFireArrow()
        {
            Instantiate(_fireArrowPrefab, _fireArrowSpawnTrm.position, _fireArrowSpawnTrm.rotation);
        }

        private void SpawnFireProjectile()
        {
            for(int i = 0; i < 10; ++i)
            {
                FireProjectile projectile = MonoGenericPool<FireProjectile>.Pop();
                projectile.transform.position = _fireProjectileSpawnTrm.position;

                projectile.SetAngle(36f * i);
            }
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
