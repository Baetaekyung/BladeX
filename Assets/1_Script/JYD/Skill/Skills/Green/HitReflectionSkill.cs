using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "HitReflectionSkill", menuName = "SO/Skill/Green/HitReflection")]
    public class HitReflectionSkill : SkillData
    {
        [SerializeField] private float knockbackForce;
        [SerializeField] private LayerMask whatIsTarget;
        private float skillRadius = 10;
                
        
        public override void Initialize()
        {
            MonoGenericPool<ShockWaveParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            if (targets == null)
            {
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position, skillRadius, whatIsTarget)
                    .Select(x => x.transform).ToArray();
            }
    
            ShockWaveParticle redWaveParticle = MonoGenericPool<ShockWaveParticle>.Pop();
            redWaveParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0, 0.5f, 0);
            
            Transform closeTarget = null;
            float minDistance = float.MaxValue;
            foreach (var item in targets)
            {
                float distance = Vector3.Distance(item.position, player.GetPlayerTransform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closeTarget = item;
                }
            }
            
            if (TryUseSkill())
            {
                if (closeTarget != null && closeTarget.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 8,
                        knockbackForce = knockbackForce,
                        knockbackDirection = (closeTarget.position - player.GetPlayerTransform.position).normalized,
                        stun = true
                    };
                    health.TakeDamage(actionData);
                    
                }
            }
            
        }
    }
}
