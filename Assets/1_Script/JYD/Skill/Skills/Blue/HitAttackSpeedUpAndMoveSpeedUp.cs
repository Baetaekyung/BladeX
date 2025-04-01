using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "HitSpeedUp", menuName = "SO/Skill/Blue/HitSpeedUp")]
    public class HitAttackSpeedUpAndMoveSpeedUp : SkillData
    {
        [Range(0.1f, 10)] [SerializeField] private int increaseAmount;

        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, Transform[] targets = null)
        {
            if(CheckSkill() == false)return;
                        
            float healthDifference = player.GetPlayerStat.GetStat(StatType.HEALTH).Value - player.GetPlayerHealth.GetCurrentHealth;
            
            var statCompo = player.GetPlayerStat;
            
            ResetStat(player);
            statCompo.AddModifier(StatType.MOVESPEED, skillName, increaseAmount * healthDifference);            
            statCompo.AddModifier(StatType.ATTACKSPEED, skillName, increaseAmount * healthDifference);      
            
            MonoGenericPool<BlueWaveParticle>.Pop().transform.position =  player.GetPlayerTransform.position + new Vector3(0,1,0);
                        
            Debug.Log(player.GetPlayerAnimator.GetAnimator.GetFloat("AttackSpeed"));
        }
        
        public override void SkillUpdate(Player player, List<Transform> targets = null)
        {
            Debug.Log(player.GetPlayerAnimator.GetAnimator.GetFloat("AttackSpeed"));
        }
        
        private void ResetStat(Player player)
        {
            player.GetPlayerStat.RemoveModifier(StatType.MOVESPEED,skillName);
            player.GetPlayerStat.RemoveModifier(StatType.ATTACKSPEED,skillName);
        }
        
        public override void ResetSkill()
        {
            
        }
        
    }
}
