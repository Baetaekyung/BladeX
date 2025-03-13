using Swift_Blade.Feeling;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class Bomb : BaseThrow
    {
        public float explosionRadius;

        public LayerMask whatIsGround;
        public LayerMask whatIsTarget;

        public CameraShakeType shakeType;

        public PoolPrefabMonoBehaviourSO explosionSO;
        private bool canExplosion;

        protected override void Start()
        {
            base.Start();

            MonoGenericPool<Explosion>.Initialize(explosionSO);
        }

        private void OnCollisionEnter(Collision other)
        {
            if ((whatIsGround & (1 << other.gameObject.layer)) != 0 ||
                (whatIsTarget & (1 << other.gameObject.layer)) != 0)
            {
                if (canExplosion)
                {
                    Explosion(other.contacts[0].point);
                }
            }
                   
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        private void Explosion(Vector3 explosionPoint)
        {
            var colliders = Physics.OverlapSphere(explosionPoint, explosionRadius, whatIsTarget);

            if (colliders.Length > 0)
            {
                var actionData = new ActionData();
                actionData.damageAmount = 40;
                colliders[0].GetComponentInChildren<IDamageble>().TakeDamage(actionData);
            }

            CameraShakeManager.Instance.DoShake(shakeType);
            
            var e = MonoGenericPool<Explosion>.Pop();
            e.transform.position = explosionPoint;

            Destroy(gameObject);
        }

        public override void SetPhysicsState(bool isActive)
        {
            base.SetPhysicsState(isActive);

            if (isActive == false)
                canExplosion = true;
        }
    }
}