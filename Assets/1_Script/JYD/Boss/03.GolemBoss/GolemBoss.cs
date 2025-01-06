using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Boss.Golem
{
    public class GolemBoss : BossBase
    {
        [Range(1, 20)] [SerializeField] private float jumpAttackRadius;
        [SerializeField] private LayerMask whatIsTarget;

        [Space]
        [SerializeField] private Collider collider;

        protected override void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                attackDestination = transform.position + transform.forward * 1f;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    bossAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }

        public void JumpAttackCast()
        {
            //CameraShakeManager.Instance.DoShake(cameraShakeType);
            
            
            Vector3 center = transform.position;
            float radius = jumpAttackRadius;
            
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, whatIsTarget);
            
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageble health))
                {
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 10,
                        knockbackDir = transform.forward,
                        knockbackDuration = 0.2f,
                        knockbackPower = 5,
                        dealer = transform,
                    };

                    health.TakeDamage(actionData);
                }
            }
        }

        public void StartManualCollider()
        {
            collider.enabled = true;
        }

        public void StopManualCollider()
        {
            collider.enabled = false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position , jumpAttackRadius);
        }
    }
}
