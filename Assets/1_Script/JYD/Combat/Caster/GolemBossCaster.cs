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
                    ActionData actionData = new ActionData(Vector3.zero,Vector3.zero,1 , transform,true);
                    
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
