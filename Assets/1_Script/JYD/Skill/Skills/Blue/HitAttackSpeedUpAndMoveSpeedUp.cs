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
        private bool useSkill;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
            ResetSkill();
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            ResetSkill();
            
            GenerateSkillText(true);
            useSkill = true;
            
            int healthDifference = Mathf.RoundToInt(player.GetPlayerStat.GetStat(StatType.HEALTH).Value -
                                                    player.GetPlayerHealth.GetCurrentHealth);
            
            float increaseSpeed = increaseAmount * healthDifference * GetColorRatio();
            
            statCompo.AddModifier(StatType.MOVESPEED, skillName ,increaseSpeed );            
            statCompo.AddModifier(StatType.ATTACKSPEED, skillName , increaseSpeed);      
            
            MonoGenericPool<BlueWaveParticle>.Pop().transform.position =  player.GetPlayerTransform.position + new Vector3(0,1,0);
        }
                
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (useSkill)
            {
                decreaseTimer += Time.deltaTime;
                if (decreaseTimer >= decreaseTime)
                {
                    GenerateSkillText(false);
                
                    ResetSkill();
                    ResetStat();
                }
            }
        }
        
        private void ResetStat()
        {
            statCompo.RemoveModifier(StatType.MOVESPEED, skillName);
            statCompo.RemoveModifier(StatType.ATTACKSPEED, skillName);
        }
        
        public override void ResetSkill()
        {
            useSkill = false;
            decreaseTimer = 0;
        }
        
    }
}
