using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering;
using DG.Tweening;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "LastChanceSkill", menuName = "SO/Skill/Green/LastChanceSkill")]
    public class LastChanceSkill : SkillData
    {
        [SerializeField] private VolumeProfile profile;
        
        [Range(0.1f, 10)] [SerializeField] private float attackIncreaseValue;
        [Range(0.1f, 10)] [SerializeField] private float attackSpeedIncreaseValue;
        [Range(0.1f, 10)] [SerializeField] private float moveSpeedIncreaseValue;
        
        [Range(0.1f, 1)] [SerializeField] private float chromaticAberrationIntensity;
        [Range(0.1f, 2)] [SerializeField] private float chromaticAberrationDuration;
        private bool canUpgrade = false;
        private bool hasSkill = false;
        
        private ChromaticAberration chromaticAberration;
        private PlayerHealth health;
        
        
        public override void Initialize()
        {
            profile.TryGet(out chromaticAberration);
        }

        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (health == null)
                health = player.GetPlayerHealth;
            
            if (health.GetCurrentHealth == 1 && canUpgrade)
            {
                hasSkill = true;
                canUpgrade = false;
                                
                DOVirtual.Float(chromaticAberration.intensity.value , chromaticAberrationIntensity,chromaticAberrationDuration ,x =>
                {
                    chromaticAberration.intensity.value = x;
                });
                
                GenerateSkillText(true);
                
                statCompo.AddModifier(StatType.DAMAGE,skillName,attackIncreaseValue);
                statCompo.AddModifier(StatType.ATTACKSPEED,skillName,attackSpeedIncreaseValue);
                statCompo.AddModifier(StatType.MOVESPEED,skillName,moveSpeedIncreaseValue);
            }
            
            if (hasSkill && canUpgrade == false && health.GetCurrentHealth > 1)
            {
                GenerateSkillText(false);
                ResetSkill();
            }
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            
        }
        
        public override void ResetSkill()
        {
            if (health.isDead)
            {
                chromaticAberration.intensity.value = 0;
            }
            else
            {
                DOVirtual.Float(chromaticAberration.intensity.value, 0, chromaticAberrationDuration, x =>
                {
                    chromaticAberration.intensity.value = x;
                });
            }

            hasSkill = false;
            canUpgrade = true;
            
            statCompo.RemoveModifier(StatType.DAMAGE,skillName);
            statCompo.RemoveModifier(StatType.ATTACKSPEED,skillName);
            statCompo.RemoveModifier(StatType.MOVESPEED,skillName);
        }
        
    }
}
