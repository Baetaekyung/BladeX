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

        private const int MAX_HEAL_AMOUNT = 2;
        
        private int skillCounter;
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                if (TryUseSkill())
                {
                    int healthAmount = Mathf.RoundToInt(healAmount * GetColorRatio());
                    player.GetPlayerHealth.TakeHeal(Mathf.Min(MAX_HEAL_AMOUNT, healthAmount));
                }
                
                skillCounter = 0;
            }
        }
    }
}
