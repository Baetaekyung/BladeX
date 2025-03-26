using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "RollingThunderSkill", menuName = "SO/Skill/Rolling/Thunder")]
    public class RollingThunderSkill : SkillData
    {
        [SerializeField] private int skillCount;
        private int skillCounter = 0;
        [SerializeField] private int skillDamage;
        [SerializeField] private float skillRadius;
        [SerializeField] private LayerMask whatIsTarget;

        public override void Initialize()
        {
            MonoGenericPool<ThunderParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, Transform[] targets = null)
        {
            if (targets == null || targets.Length == 0)
            {
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position , skillRadius ,whatIsTarget).Select(x => x.transform).ToArray();
            }

            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                skillCounter = 0;
                
                foreach (var item in targets)
                {
                    if (item.TryGetComponent(out BaseEnemyHealth health))
                    {
                        ActionData actionData = new ActionData();
                        actionData.damageAmount = skillDamage;
                        actionData.stun = true;
                        
                        health.TakeDamage(actionData);
                    }
                
                    ThunderParticle th = MonoGenericPool<ThunderParticle>.Pop();
                    th.transform.position = item.transform.position + new Vector3(0,1,0);
                }
            }
            
            
        }
    }
}
