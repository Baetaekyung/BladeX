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
        
        private bool usingSkill = true;
        private bool isSkillUp = false;

        public override void Initialize()
        {
            MonoGenericPool<RedWaveParticle>.Initialize(skillParticle);
            usingSkill = false;
            isSkillUp = false;
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            if (usingSkill || isSkillUp) return;
            
            MonoGenericPool<RedWaveParticle>.Pop().transform.position = player.GetPlayerTransform.position + new Vector3(0,1,0);
            usingSkill = true;
        }
        
        public override void SkillUpdate(Player player,  IEnumerable<Transform> targets = null)
        {
            if (usingSkill)
            {
                if (isSkillUp == false)
                {
                    isSkillUp = true;
                    
                    GenerateSkillText(true);
                    statCompo.AddModifier(statType , skillName , attackIncreaseAmount * GetColorRatio());
                }
                else
                {
                    increaseTimer += Time.deltaTime;
                }
            }
            
            if (increaseTimer >= increaseTime)
            {
                GenerateSkillText(false);
                ResetSkill();
            }
        }

        public override void ResetSkill()
        {
            statCompo.RemoveModifier(statType , skillName);
            
            usingSkill = false;
            isSkillUp = false;
            increaseTimer = 0;
        }
        
    }
}
