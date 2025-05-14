using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "MaxHealthStrikeSkill", menuName = "SO/Skill/Green/MaxHealthStrikeSkill")]
    public class MaxHealthStrikeSkill : SkillData
    { 
        [Range(0.1f, 5f)] [SerializeField]  private float attackIncreaseAmount;
                
        public override void Initialize()
        {
            MonoGenericPool<GreenWaveParticle>.Initialize(skillParticle);    
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            if (player.GetPlayerHealth.IsFullHealth && targets != null)
            {
                GenerateSkillText(true);
                
                GreenWaveParticle greenWaveParticle = MonoGenericPool<GreenWaveParticle>.Pop();
                greenWaveParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0, 0.5f, 0);
                
                ApplyDamage(targets);
            }
        }

        private void ApplyDamage(IEnumerable<Transform> targets)
        {
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData();
                    float damage = attackIncreaseAmount * GetColorRatio();
                    
                        
                    actionData.damageAmount = damage;
                    health.TakeDamage(actionData);
                }
            }
        }
    }
}
