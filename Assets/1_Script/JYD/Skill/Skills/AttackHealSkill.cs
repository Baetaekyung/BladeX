using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "AttackHealSkill", menuName = "SO/Skill/Attack/Heal")]
    public class AttackHealSkill : SkillData
    {
        [SerializeField] private int skillCount;
        private int skillCounter;
        
        [SerializeField] private int healAmount;
        public override void UseSkill(Player player, Transform[] targets = null)
        {
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                skillCounter = 0;
                player.GetPlayerHealth.TakeHeal(healAmount);
            }
            
        }
    }
}
