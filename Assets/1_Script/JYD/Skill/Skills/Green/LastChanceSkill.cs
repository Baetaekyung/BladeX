using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public class LastChanceSkill : SkillData
    {
        [SerializeField] private VolumeProfile profile;
        
        [Range(0.1f, 10)] [SerializeField] private float attackIncreaseValue;
        [Range(0.1f, 10)] [SerializeField] private float attackSpeedIncreaseValue;
        [Range(0.1f, 10)] [SerializeField] private float moveSpeedIncreaseValue;
        private bool canUpgrade = false;
        
        private ChromaticAberration chromaticAberration;
        
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (canUpgrade == false && player.GetPlayerHealth.GetCurrentHealth > 1)
            {
                ResetSkill();
                
            }
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if (player.GetPlayerHealth.GetCurrentHealth <= 1 && canUpgrade)
            {
                canUpgrade = false;
                
                statCompo.AddModifier(StatType.DAMAGE,skillName,attackIncreaseValue);
                statCompo.AddModifier(StatType.ATTACKSPEED,skillName,attackSpeedIncreaseValue);
                statCompo.AddModifier(StatType.MOVESPEED,skillName,moveSpeedIncreaseValue);
            }      
        }
        
        public override void ResetSkill()
        {
            canUpgrade = true;
            
            statCompo.RemoveModifier(StatType.DAMAGE,skillName);
            statCompo.RemoveModifier(StatType.ATTACKSPEED,skillName);
            statCompo.RemoveModifier(StatType.MOVESPEED,skillName);
        }
        
    }
}
