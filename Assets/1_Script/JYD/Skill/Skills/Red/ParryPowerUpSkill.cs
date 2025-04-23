using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryPowerUpSkill", menuName = "SO/Skill/Red/ParryPowerUp")]
    public class ParryPowerUpSkill : SkillData
    {
        [Range(0.1f,10f)]  [SerializeField] private float attackIncreaseAmount;
        [Range(0.1f,10f)] [SerializeField] private float increaseTime;
        private float increaseTimer;
        private bool canUseSkill = true;
        private bool isSkillUp = false;

        public override void Initialize()
        {
            MonoGenericPool<RedWaveParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            MonoGenericPool<RedWaveParticle>.Pop().transform.position = player.GetPlayerTransform.position + new Vector3(0,1,0);
            
            canUseSkill = true;
        }

        public override void SkillUpdate(Player player,  IEnumerable<Transform> targets = null)
        {
            if (canUseSkill)
            {
                if (isSkillUp == false)
                {
                    isSkillUp = true;
                    player.GetPlayerStat.AddModifier(statType , skillName , attackIncreaseAmount * GetColorRatio());
                }
                
                increaseTimer += Time.deltaTime;
                                
                if (increaseTimer >= increaseTime)
                {
                    ResetSkill();
                    player.GetPlayerStat.RemoveModifier(statType , skillName);
                }
                
            }
        }

        public override void ResetSkill()
        {
            canUseSkill = false;
            isSkillUp = false;
            increaseTimer = 0;
        }
        
    }
}
