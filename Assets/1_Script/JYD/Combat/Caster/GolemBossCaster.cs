using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class GolemBossCaster : BaseBossCaster
    {
        [Range(1, 20)] [SerializeField] private float jumpAttackRadius;
        
        
        public void JumpAttackCast()
        {
            //CameraShakeManager.Instance.DoShake(cameraShakeType);
            
            Vector3 center = transform.position;
            float radius = jumpAttackRadius;
            
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, targetLayer);
            
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

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position , jumpAttackRadius);
        }
    }
}
