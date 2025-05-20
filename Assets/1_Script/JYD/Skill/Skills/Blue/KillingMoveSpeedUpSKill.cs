using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "KillingMoveSpeedUpSKill",menuName = "SO/Skill/Blue/KillingMoveSpeedUpSKill")]
    public class KillingMoveSpeedUpSKill : SkillData
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO directionArrow; 
        
        [Range(0.1f, 5)] [SerializeField] private float increaseValue;
        [Range(0.1f, 5)] [SerializeField] private float increaseTime;
        private float timer = 0;
        private DirectionArrowParticle directionArrowParticle;

        private bool useSkill;
        
        public override void Initialize()
        {
            MonoGenericPool<BlueWaveParticle>.Initialize(skillParticle);
            MonoGenericPool<DirectionArrowParticle>.Initialize(directionArrow);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemyHealth health) && health.isDead)
                {
                    useSkill = true;
                                        
                    GenerateSkillText(useSkill);
                    PushDirectionArrowParticle();
                    
                    BlueWaveParticle blueWaveParticle = MonoGenericPool<BlueWaveParticle>.Pop();
                    blueWaveParticle.transform.SetParent(player.GetPlayerTransform);
                    blueWaveParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0,0.5f,0);
                    
                    directionArrowParticle = MonoGenericPool<DirectionArrowParticle>.Pop();
                    directionArrowParticle.SetFollowTransform(player.GetPlayerTransform);
                    directionArrowParticle.transform.position = player.GetPlayerTransform.position + 
                                                                new Vector3(0,1.7f,0);
                    
                    statCompo.AddModifier(statType, skillName, increaseValue * GetColorRatio());
                                        
                    break;
                }
            }
        
        }

        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (useSkill)
            {
                timer += Time.deltaTime;
                if (timer >= increaseTime)
                {
                    useSkill = false;
                    GenerateSkillText(useSkill);
                    ResetSkill();
                    PushDirectionArrowParticle();
                }
            }
        }

        public override void ResetSkill()
        {
            timer = 0;
            statCompo.RemoveModifier(statType , skillName);
        }
        
        private void PushDirectionArrowParticle()
        {
            if (directionArrowParticle != null)
            {
                MonoGenericPool<DirectionArrowParticle>.Push(directionArrowParticle);
                directionArrowParticle = null;
            }
        }
        
    }
}
