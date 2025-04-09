using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class GolemEnemyCaster : BaseEnemyCaster
    {
        [Range(1, 20)] [SerializeField] private float jumpAttackRadius;
                
        public void JumpAttackCast()
        {
            //CameraShakeManager.Instance.DoShake(cameraShakeType);
            
            Vector3 center = transform.position;
            float radius = jumpAttackRadius;
            
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, whatIsTarget);
            
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IHealth health))
                {
                    ActionData actionData = new ActionData(Vector3.zero,Vector3.zero,1 , true);
                    
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
