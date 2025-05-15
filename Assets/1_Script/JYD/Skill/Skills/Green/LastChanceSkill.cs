using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Rendering;
using DG.Tweening;
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
        
        private ChromaticAberration chromaticAberration;
        
        public override void Initialize()
        {
            profile.TryGet(out chromaticAberration);
        }

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
                GenerateSkillText(true);
                
                canUpgrade = false;
                    
                DOVirtual.Float(chromaticAberration.intensity.value , chromaticAberrationIntensity,chromaticAberrationDuration ,x =>
                {
                    chromaticAberration.intensity.value = x;
                });
                
                statCompo.AddModifier(StatType.DAMAGE,skillName,attackIncreaseValue);
                statCompo.AddModifier(StatType.ATTACKSPEED,skillName,attackSpeedIncreaseValue);
                statCompo.AddModifier(StatType.MOVESPEED,skillName,moveSpeedIncreaseValue);
            }      
        }
        
        public override void ResetSkill()
        {
            GenerateSkillText(false);
            
            DOVirtual.Float(chromaticAberration.intensity.value ,0 ,chromaticAberrationDuration ,x =>
            {
                chromaticAberration.intensity.value = x;
            });
            
            canUpgrade = true;
            
            statCompo.RemoveModifier(StatType.DAMAGE,skillName);
            statCompo.RemoveModifier(StatType.ATTACKSPEED,skillName);
            statCompo.RemoveModifier(StatType.MOVESPEED,skillName);
        }
        
    }
}
