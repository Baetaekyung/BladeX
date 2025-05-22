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
        [Range(0.1f, 8f)] [SerializeField]  private float attackIncreaseAmount;
        private bool useSkill = false;
                
        public override void Initialize()
        {
            MonoGenericPool<GreenWaveParticle>.Initialize(skillParticle);
            useSkill = false;
        }

        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (player.GetPlayerHealth.IsFullHealth && useSkill != false)
            {
                useSkill = true;
                
                GenerateSkillText(true);
                AddStat();
            }
            else if(!player.GetPlayerHealth.IsFullHealth && useSkill)
            {
                ResetSkill();
            }
        }

        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            
        }
        
        private void AddStat()
        {
            statCompo.AddModifier(statType , skillName,attackIncreaseAmount + GetColorRatio());
        }
        
        public override void ResetSkill()
        {
            statCompo.RemoveModifier(statType , skillName);
        }
    }
}
