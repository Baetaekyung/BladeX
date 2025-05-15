using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "RollingThunderSkill", menuName = "SO/Skill/Yellow/RollingThunder")]
    public class RollingThunderSkill : SkillData
    {
        [SerializeField] private int skillCount;
        private int skillCounter = 0;
        [SerializeField] private float skillRadius;
        [SerializeField] private LayerMask whatIsTarget;

        private bool hasGeneratedText = false;
        
        public override void Initialize()
        {
            MonoGenericPool<LightingSparkParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if (targets == null || !targets.Any())
            {
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position, skillRadius, whatIsTarget)
                    .Select(x => x.transform);
            }
                        
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                skillCounter = 0;
                if (TryUseSkill())
                {
                    foreach (var item in targets)
                    {
                        if (item.TryGetComponent(out BaseEnemyHealth health))
                        {
                            ActionData actionData = new ActionData();
                            actionData.stun = true;
                            actionData.hurtType = 1;
                            
                            health.TakeDamage(actionData);
                        }

                        if (hasGeneratedText == false)
                        {
                            GenerateSkillText(true);
                            hasGeneratedText = true;
                        }
                        
                        LightingSparkParticle th = MonoGenericPool<LightingSparkParticle>.Pop();
                        th.transform.position = player.GetPlayerTransform.position + new Vector3(0,0.4f,0);
                    }
                    
                    hasGeneratedText = false;
                }
                
            }
        
        }
            
    }
}
