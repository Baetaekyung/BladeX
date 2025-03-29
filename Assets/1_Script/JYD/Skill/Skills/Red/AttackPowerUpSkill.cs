using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "AttackPowerUpSkill", menuName = "SO/Skill/Red/AttackPowerUp")]
    public class AttackPowerUpSkill : SkillData
    {
        [Range(0.1f, 10)] [SerializeField] private float radius;
        [SerializeField] private LayerMask whatIsTarget;
        [Range(1, 10)] [SerializeField] private int targetCount;
        [Range(1f, 10f)] [SerializeField] private float increaseValue;
        [Tooltip("색깔 스탯의 영향을 얼마나 받을지")] [Range(1f, 10f)] [SerializeField] private float ratio;
        
        private bool isUpgrade;
        
        public override void SkillUpdate(Player player, List<Transform> targets = null)
        {
            targets = Physics.OverlapSphere(player.GetPlayerTransform.position, radius, whatIsTarget)
                .Select(x => x.transform).ToList();
            
            if (isUpgrade == false && targets.Count >= targetCount)
            {
                isUpgrade = true;
                
                Debug.Log("강해짐");
                
                player.GetPlayerStat.GetStat(StatType).AddModifier("Damage" , increaseValue);
            }
            else if(isUpgrade && targets.Count < targetCount)
            {
                isUpgrade = false;
                
                Debug.Log("약해짐");
                
                player.GetPlayerStat.GetStat(StatType).RemoveModifier("Damage");
            }
            
        }

        public override void UseSkill(Player player, Transform[] targets = null)
        {
            
        }
        
    }
}
