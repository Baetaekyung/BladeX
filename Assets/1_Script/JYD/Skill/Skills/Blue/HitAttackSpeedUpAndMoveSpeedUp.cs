using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "HitSpeedUp", menuName = "SO/Skill/Blue/HitSpeedUp")]
    public class HitAttackSpeedUpAndMoveSpeedUp : SkillData
    {
        [Range(0.1f, 20f)] [SerializeField] private float increaseAmount;
        [Range(0.1f,10f)][SerializeField] private float decreaseTime;
                
        private float decreaseTimer = 0;
        private bool isOnSkill;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            if(isOnSkill)return;
            GenerateSkillText(true);
            
            isOnSkill = true;
            ResetStat();
            
            int healthDifference = Mathf.RoundToInt(player.GetPlayerStat.GetStat(StatType.HEALTH).Value -
                                                    player.GetPlayerHealth.GetCurrentHealth);
            float increaseSpeed = increaseAmount * healthDifference * GetColorRatio();
            
            statCompo.AddModifier(StatType.MOVESPEED, skillName ,increaseSpeed );            
            statCompo.AddModifier(StatType.ATTACKSPEED, skillName , increaseSpeed);      
            
            MonoGenericPool<BlueWaveParticle>.Pop().transform.position =  player.GetPlayerTransform.position + new Vector3(0,1,0);
            
        }
        
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (isOnSkill)
            {
                decreaseTimer += Time.deltaTime;
            }
            
            if (decreaseTimer >= decreaseTime)
            {
                ResetSkill();
                ResetStat();
            }
        }
        
        private void ResetStat()
        {
            statCompo.RemoveModifier(StatType.MOVESPEED, skillName);
            statCompo.RemoveModifier(StatType.ATTACKSPEED, skillName);
        }
        
        public override void ResetSkill()
        {
            GenerateSkillText(false);
            
            isOnSkill = false;
            decreaseTimer = 0;
        }
        
    }
}
