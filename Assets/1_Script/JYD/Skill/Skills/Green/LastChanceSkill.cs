using System.Collections.Generic;
using DG.Tweening;
using Swift_Blade.Pool;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
                canUpgrade = false;
                    
                Debug.Log("¤µ¤´¤²");
                
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
