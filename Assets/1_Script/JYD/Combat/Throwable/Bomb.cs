using System;
using Swift_Blade.Feeling;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class Bomb : BaseThrow
    {
        public float explosionRadius;
        
        public LayerMask whatIsGround;
        public LayerMask whatIsTarget;

        public CameraShakeType shakeType;
        private bool canExplosion;
        
        private void Explosion()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position , explosionRadius , whatIsTarget);
            if (colliders.Length > 0)
            {
                ActionData actionData = new ActionData();
                actionData.damageAmount = 40;
                colliders[0].GetComponentInChildren<IDamageble>().TakeDamage(actionData);
                CameraShakeManager.Instance.DoShake(shakeType);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if ((whatIsGround & (1 << other.gameObject.layer)) != 0)
            {
                if (canExplosion)
                    Explosion();
            }
        }

        public override void SetPhysicsState(bool isActive)
        {
            base.SetPhysicsState(isActive);

            if (isActive == false)
                canExplosion = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position , explosionRadius);
        }
    }
}
