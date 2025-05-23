using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "AttackAttackSpeedUpSkill",menuName = "SO/Skill/Blue/AttackAttackSpeedUp")]
    public class AttackAttackSpeedUpSkill : SkillData
    {
        [Range(0f,5f)] [SerializeField] private float increaseValue;
        [Range(0.1f,10f)] [SerializeField] private float increaseMaxValue;
        private float currentIncreaseValue;
        
        [Range(0.1f,5f)][SerializeField] private float decreaseTime;
        private float decreaseTimer = 0;
        
        private const int PARTICLE_INTERVAL_DIVIDE = 8;
           
        private readonly float[] divideValue = new float[PARTICLE_INTERVAL_DIVIDE];
        private readonly bool[] isTriggerParticle = new bool[PARTICLE_INTERVAL_DIVIDE];

        private bool useSkill = false;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
            CalculateParticleTime();
            
            useSkill = false;
            decreaseTimer = 0;
            //ResetSkill();
        }
        
        private void CalculateParticleTime()
        {
            float interval = increaseMaxValue / PARTICLE_INTERVAL_DIVIDE;
                        
            for (int i = 1; i <= PARTICLE_INTERVAL_DIVIDE; i++)
            {
                //Up to 2 decimal places are allowed
                divideValue[i - 1] = (Mathf.Round(interval * i * 100) / 100);
            }
            
        }
        
        private void TryAttackSpeedUp()
        {
            useSkill = true;
            
            decreaseTimer = 0; 
            currentIncreaseValue += increaseValue;
            currentIncreaseValue = Mathf.Min(currentIncreaseValue , increaseMaxValue);
            
            statCompo.RemoveModifier(statType , skillName);    
            statCompo.AddModifier(statType , skillName, currentIncreaseValue);
        }
        
        private void TryPlayParticle(Player player)
        {
            for (int i = divideValue.Length - 1; i >= 0 ; i--)
            {
                if (divideValue[i] <= currentIncreaseValue && isTriggerParticle[i] == false)
                {
                    isTriggerParticle[i] = true;
                    BlueWaveParticle waveParticle = MonoGenericPool<BlueWaveParticle>.Pop();
                    waveParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0,1,0);
                    
                    break;
                }
            }
            
        }
        
        public override void SkillUpdate(Player player,  IEnumerable<Transform> targets = null)
        {
            if (useSkill)
            {
                decreaseTimer += Time.deltaTime;
                if (decreaseTimer >= decreaseTime)
                {
                    GenerateSkillText(false);
                    ResetSkill();                
                }
            }
            
        }

        public override void ResetSkill()
        {
            useSkill = false;
            
            statCompo.RemoveModifier(statType ,skillName);
            decreaseTimer = 0;
            currentIncreaseValue = 0;
            
            for (int i = 0; i < isTriggerParticle.Length; i++)
            {
                isTriggerParticle[i] = false;
            }
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            GenerateSkillText(true);
            
            TryPlayParticle(player);
            TryAttackSpeedUp();
        }
        
    }
}
