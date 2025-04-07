using System.Collections.Generic;
using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "AttackHealSkill", menuName = "SO/Skill/Red/AttackHeal")]
    public class AttackHealSkill : SkillData
    {
        [SerializeField] private int skillCount;
        [SerializeField] private int healAmount;
              
        private int skillCounter;
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                float randomValue = UnityEngine.Random.Range(0,100);
                if (randomValue < random)
                {
                    player.GetPlayerHealth.TakeHeal(healAmount);    
                }
                
                skillCounter = 0;
            }
        }
    }
}
