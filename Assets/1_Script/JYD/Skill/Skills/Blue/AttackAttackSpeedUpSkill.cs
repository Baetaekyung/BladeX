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
            
            //������ �����޴� ���� ����� ���� ���� ����?�̷��� �ؾ� �� ��ų���� �ִ�� ������ ������ �����Ҽ� ����.
            //�׳� ���ȿ��� ���� ���ͼ� ���Ҽ��� �ִµ� �׷����ϸ� �ٸ� ��ų���� ����� ���� ������ ����.
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
