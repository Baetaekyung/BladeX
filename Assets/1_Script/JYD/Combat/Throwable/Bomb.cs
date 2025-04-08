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
        private bool canExplosion;
        
        private readonly Collider[] targets = new Collider[10];
        
        protected override void Start()
        {
            base.Start();

            MonoGenericPool<ExplosionParticle>.Initialize(explosionSO);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (canExplosion)
            {
                Explosion(other.contacts[0].point);
            }
        }
        
        private void Explosion(Vector3 explosionPoint)
        {
            int counts = Physics.OverlapSphereNonAlloc(explosionPoint, explosionRadius, targets, whatIsTarget);
            
            var actionData = new ActionData
            {
                damageAmount = 1,
                stun = true
            };
            
            for (int i = 0; i < counts; i++)
            {
                var target = targets[i].GetComponentInChildren<IHealth>();
                if (target != null)
                {
                    target.TakeDamage(target is BaseEnemyHealth? new ActionData(Vector3.zero , Vector3.zero , 5 , true) : actionData);
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