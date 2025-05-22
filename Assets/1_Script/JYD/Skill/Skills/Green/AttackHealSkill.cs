using System.Collections.Generic;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "AttackHealSkill", menuName = "SO/Skill/Green/AttackHeal")]
    public class AttackHealSkill : SkillData
    {
        [SerializeField] private int skillCount;
        [SerializeField] private int healAmount;

        private const int HEAL_AMOUNT = 1;
        
        private int skillCounter;
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            if (player.GetPlayerHealth.IsFullHealth) return;
            
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                if (TryUseSkill())
                {
                    GenerateSkillText(true);
                    
                    player.GetPlayerHealth.TakeHeal(HEAL_AMOUNT);
                }
                
                skillCounter = 0;
            }
            
        }
    }
}
