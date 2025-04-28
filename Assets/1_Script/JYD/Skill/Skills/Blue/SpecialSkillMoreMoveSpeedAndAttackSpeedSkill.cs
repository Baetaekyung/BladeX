using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "SpecialSkillMoreMoveSpeedAndAttackSpeedSkill", menuName = "SO/Skill/Blue/SpecialSkillMoreMoveSpeedAndAttackSpeedSkill")]
    public class SpecialSkillMoreMoveSpeedAndAttackSpeedSkill : SkillData
    {
        [SerializeField] [Range(0.1f , 10)]private float increaseValue;
        [SerializeField] [Range(0.1f , 10)]private float increaseTime;
        private float timer;

        private bool useSkill;
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            useSkill = true;
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
                    statCompo.RemoveModifier(statType , skillName);        
                }
            }
        }
        
        
    }
}
