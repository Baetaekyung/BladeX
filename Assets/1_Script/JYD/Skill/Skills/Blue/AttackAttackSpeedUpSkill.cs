using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "AttackAttackSpeedUpSkill",menuName = "SO/Skill/Blue/AttackAttackSpeedUp")]
    public class AttackAttackSpeedUpSkill : SkillData
    {
        [Range(0f,0.2f)] [SerializeField] private float increaseValue;
        [Range(0.1f,1f)] [SerializeField] private float increaseMaxValue;
        private float currentIncreaseValue;
        
        [Range(0.1f,1f)][SerializeField] private float decreaseTime;
        private float decreaseTimer = 0;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
            
            decreaseTimer = 0;
        }
        
        public override void UseSkill(Player player, Transform[] targets = null)
        { 
            decreaseTimer = 0; 
            BlueWaveParticle waveParticle = MonoGenericPool<BlueWaveParticle>.Pop();
            waveParticle.transform.position = player.GetPlayerTransform.position;
            
            currentIncreaseValue += increaseValue;
            currentIncreaseValue = Mathf.Min(currentIncreaseValue , increaseMaxValue);
            
            //이전에 적용햇던 값을 지우고 현재 값을 적용?이렇게 해야 이 스킬에서 최대로 증가할 스탯을 설정할수 있음.
            //그냥 스탯에서 값을 빼와서 비교할수도 있는데 그렇게하면 다른 스킬에서 적용된 값을 영향을 받음.
            player.GetPlayerStat.RemoveModifier(StatType.ATTACKSPEED , "AttackAttackSpeedUpSkill");    
            player.GetPlayerStat.AddModifier(StatType.ATTACKSPEED , "AttackAttackSpeedUpSkill",currentIncreaseValue);    
            
        }
        
        public override void SkillUpdate(Player player, List<Transform> targets = null)
        {
            decreaseTimer += Time.deltaTime;
            if (decreaseTimer >= decreaseTime)
            {
                decreaseTimer = 0;
                currentIncreaseValue = 0;
                player.GetPlayerStat.RemoveModifier(StatType.ATTACKSPEED , "AttackAttackSpeedUpSkill");    
            }
            
        }
        
    }
}
