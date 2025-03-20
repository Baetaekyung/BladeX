using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryHeal", menuName = "SO/Skill/Hit/Heal")]
    public class HitHealSkill : SkillData
    {
        [Range(1, 10)] public int healAmount;
        [Tooltip("Èú µÉ È®·ü.")][Range(1, 100)] public int randomAmount;
        
        private PlayerHealth playerHealth;
        
        public override void UseSkill(Transform player)
        {
            if(playerHealth == null)
                playerHealth = player.GetComponentInChildren<PlayerHealth>();
            
            if(Random.Range(1, 100) < randomAmount)
                playerHealth.TakeHeal(healAmount);
        }
        
    }
}

