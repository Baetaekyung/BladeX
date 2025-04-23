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
            if (TryUseSkill() && player.GetPlayerHealth.IsFullHealth && targets != null)
            {
                GreenWaveParticle greenWaveParticle = MonoGenericPool<GreenWaveParticle>.Pop();
                greenWaveParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0, 1, 0);
                foreach (var item in targets)
                {
                    if (item.TryGetComponent(out BaseEnemyHealth health))
                    {
                        ActionData actionData = new ActionData();
                        actionData.damageAmount = attackIncreaseAmount * GetColorRatio();
                        health.TakeDamage(actionData);
                    }
                    
                }
            }
        }
             
    }
}
