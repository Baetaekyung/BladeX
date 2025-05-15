using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryknockbackSkill", menuName = "SO/Skill/Red/ParryKnockback")]
    public class ParryKnockbackSkill : SkillData
    {
        [SerializeField] private float defualtKnockbackForce;
        [SerializeField] private float defaultKnockbackRadius;
        
        [SerializeField] private float maxKnockbackForce;
        [SerializeField] private float maxKnockbackRadius;
        
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
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position , Mathf.Min(maxKnockbackRadius,defaultKnockbackRadius + GetColorRatio()) ,whatIsTarget).Select(x => x.transform).ToArray();
            }
            
            foreach (var item in targets)
            {
                GenerateSkillText(true);
                
                if (item.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData
                    {
                        knockbackForce = Mathf.Min(maxKnockbackForce, defualtKnockbackForce * GetColorRatio()),
                        knockbackDirection = (item.position - player.GetPlayerTransform.position).normalized,
                        stun = true
                    };
                    
                    health.TakeDamage(actionData);
                    
                }
            }
            
            CircleWindParticle circleWindParticle = MonoGenericPool<CircleWindParticle>.Pop();
            circleWindParticle.transform.position = player.GetPlayerTransform.position;
            
            
        }
                
    }
}
