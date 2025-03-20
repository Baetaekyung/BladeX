using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryHeal", menuName = "SO/Skill/Parry/Heal")]
    public class ParryHealthSKill : SkillData
    {
        [Range(1, 10)] public float healAmount;
        private PlayerHealth playerHealth;
        
        public override void UseSkill(Transform player)
        {
            if(playerHealth == null)
                playerHealth = player.GetComponentInChildren<PlayerHealth>();
            
            playerHealth.TakeHeal(healAmount);
        }
        
    }
}

