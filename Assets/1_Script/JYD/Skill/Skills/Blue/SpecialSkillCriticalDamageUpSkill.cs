using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "SpecialSkillCriticalDamageUpSkill", menuName = "SO/Skill/Blue/SpecialSkillCriticalDamageUpSkill")]
    public class SpecialSkillCriticalDamageUpSkill : SkillData
    {
        [SerializeField] [Range(0.1f , 10)]private float increaseValue;
        [SerializeField] [Range(0.1f , 10)]private float increaseTime;
        private float timer;
        
        private bool useSkill;
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            useSkill = true;
            GenerateSkillText(useSkill);
                        
            statCompo.AddModifier(statType , skillName , increaseValue);                                       
        }

        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (useSkill)
            {
                timer += Time.deltaTime;
                if (timer >= increaseTime)
                {
                    timer = 0;
                    useSkill = false;
                    GenerateSkillText(useSkill);
                    
                    statCompo.RemoveModifier(statType , skillName);        
                }
            }
        }
        
        
    }
}
