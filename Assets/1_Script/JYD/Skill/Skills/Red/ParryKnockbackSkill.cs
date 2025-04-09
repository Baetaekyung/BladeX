using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryknockbackSkill", menuName = "SO/Skill/Red/ParryKnockback")]
    public class ParryKnockbackSkill : SkillData
    {
        [SerializeField] private float knockbackForce;
        [SerializeField] private float knockbackRadius;
        [SerializeField] private LayerMask whatIsTarget;
        
        public override void Initialize()
        {
            MonoGenericPool<CircleWindParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if(TryUseSkill() == false)return;
            
            if (targets == null)
            {
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position , knockbackRadius ,whatIsTarget).Select(x => x.transform).ToArray();
            }
            
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData();
                    actionData.knockbackForce = knockbackForce;
                    actionData.knockbackDirection = (item.position - player.GetPlayerTransform.position).normalized;
                    actionData.stun = true;
                    
                    health.TakeDamage(actionData);
                    
                }
            }
            
            CircleWindParticle circleWindParticle = MonoGenericPool<CircleWindParticle>.Pop();
            circleWindParticle.transform.position = player.GetPlayerTransform.position;
            
            
        }
                
    }
}
