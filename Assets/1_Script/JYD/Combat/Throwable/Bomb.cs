using Swift_Blade.Combat.Health;
using Swift_Blade.Feeling;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class Bomb : BaseThrow
    {
        public LayerMask whatIsTarget;
        public CameraShakeType shakeType;
        public PoolPrefabMonoBehaviourSO explosionSO;

        public float explosionRadius;
        public int defaultDamage = 1;
        public int enemyDamage = 5;
        
        private bool canExplosion;
        private bool hasExploded; // 무한루프 방지용 플래그
        private readonly Collider[] targets = new Collider[10];
        
        private LayerMask whatIsEnemy;
        
        protected override void Start()
        {
            base.Start();
            whatIsEnemy = LayerMask.NameToLayer("Enemy");
                        
            MonoGenericPool<ExplosionParticle>.Initialize(explosionSO);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (CheckEnemyLayer(other) && canExplosion)
            {
                Explosion(other.GetContact(0).point);
            }
        }

        
        private bool CheckEnemyLayer(Collision other)
        {
            return other.gameObject.layer != whatIsEnemy;
        }
        private void Explosion(Vector3 explosionPoint)
        {
            if (canExplosion == false || hasExploded) 
                return;
            
            canExplosion = false;
            hasExploded = true;
            
            int counts = Physics.OverlapSphereNonAlloc(explosionPoint, explosionRadius, targets, whatIsTarget);
            
            for (int i = 0; i < counts; i++)
            {
                var target = targets[i].GetComponentInChildren<IHealth>();
                if (target != null)
                {
                    var defaultAction = new ActionData
                    {
                        damageAmount = defaultDamage,
                        stun = true
                    };
                    
                    var action = (target is BaseEnemyHealth)
                        ? new ActionData(Vector3.zero, Vector3.zero, enemyDamage, true)
                        : defaultAction;

                    target.TakeDamage(action);
                }
                else if (targets[i].TryGetComponent(out Bomb otherBomb))
                {
                    otherBomb.SetPhysicsState(false);
                    otherBomb.SetDirection(Vector3.zero);
                    otherBomb.Explosion(otherBomb.transform.position);
                }
            }

            CameraShakeManager.Instance.DoShake(shakeType);

            var e = MonoGenericPool<ExplosionParticle>.Pop();
            e.transform.position = explosionPoint;
            
            Destroy(gameObject);
        }

        public override void SetPhysicsState(bool isActive)
        {
            base.SetPhysicsState(isActive);
            if (isActive == false)
                canExplosion = true;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
