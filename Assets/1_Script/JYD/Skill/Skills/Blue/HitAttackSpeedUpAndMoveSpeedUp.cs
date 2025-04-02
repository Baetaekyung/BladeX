using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "HitSpeedUp", menuName = "SO/Skill/Blue/HitSpeedUp")]
    public class HitAttackSpeedUpAndMoveSpeedUp : SkillData
    {
        [Range(0.1f, 10)] [SerializeField] private float increaseAmount;
        [Range(0.1f,10f)][SerializeField] private float decreaseTime;
        private float decreaseTimer = 0;
        private bool isOnSkill;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
            ResetSkill();
        }
        
        public override void UseSkill(Player player, Transform[] targets = null)
        {
            if(isOnSkill)return;
            
            isOnSkill = true;
            
            float healthDifference = player.GetPlayerStat.GetStat(StatType.HEALTH).Value - player.GetPlayerHealth.GetCurrentHealth;
            
            var statCompo = player.GetPlayerStat;
             
            ResetStat(player);
            
            statCompo.AddModifier(StatType.MOVESPEED, skillName , increaseAmount * healthDifference);            
            statCompo.AddModifier(StatType.ATTACKSPEED, skillName , increaseAmount * healthDifference);      
                        
            MonoGenericPool<BlueWaveParticle>.Pop().transform.position =  player.GetPlayerTransform.position + new Vector3(0,1,0);
        }
        
        public override void SkillUpdate(Player player, List<Transform> targets = null)
        {
            if (isOnSkill)
            {
                decreaseTimer += Time.deltaTime;
            }
            
            if (decreaseTimer >= decreaseTime)
            {
                ResetSkill();
                ResetStat(player);
            }
        }
        
        private void ResetStat(Player player)
        {
            player.GetPlayerStat.RemoveModifier(StatType.MOVESPEED, skillName);
            player.GetPlayerStat.RemoveModifier(StatType.ATTACKSPEED, skillName);
        }
        
        public override void ResetSkill()
        {
            isOnSkill = false;
            decreaseTimer = 0;
        }
        
    }
}
