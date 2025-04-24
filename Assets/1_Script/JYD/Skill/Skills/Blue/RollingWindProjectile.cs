using System.Collections.Generic;
using DG.Tweening;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "RollingWindProjectileSkill", menuName = "SO/Skill/Blue/WindProjectile")]
    public class RollingWindProjectile : SkillData
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO blueCircle;
        
        [SerializeField] private int skillCount;
        
        private readonly Vector3[] directions = new Vector3[4];
        private int skillCounter = 0;
        
        private int projectileCount = 1;
        
        private const int MIN_SKILL_COUNT = 1;
        private const int MAX_SKILL_COUNT = 4;

        private const float EXECUTE_DELAY = 0.1f;
        private const float FIRE_DELAY = 0.075f;
        
        public override void Initialize()
        {
            MonoGenericPool<WindProjectileParticle>.Initialize(skillParticle);
            MonoGenericPool<BlueCircle>.Initialize(blueCircle);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if(TryUseSkill() == false)return;
            
            DOVirtual.DelayedCall(EXECUTE_DELAY , () =>
            {
                if (directions == null || directions.Length != skillCount)
                {
                    directions[0] = player.GetPlayerTransform.forward;
                    directions[1] = player.GetPlayerTransform.right;
                    directions[2] = -player.GetPlayerTransform.right;
                    directions[3] = -player.GetPlayerTransform.forward;
                }
            
                ++skillCounter;
                
                int count = Mathf.Clamp(Mathf.FloorToInt(GetColorRatio()), MIN_SKILL_COUNT, MAX_SKILL_COUNT);
                
                if (skillCounter >= skillCount)
                {
                    BlueCircle blueCircle =  MonoGenericPool<BlueCircle>.Pop();
                    blueCircle.transform.SetParent(player.GetPlayerTransform);
                    blueCircle.transform.transform.position = player.GetPlayerTransform.position + new Vector3(0,0.5f,0);
                    
                    DOVirtual.DelayedCall(FIRE_DELAY , () =>
                    {
                        Fire(player, count);
                        skillCounter = 0;
                    });
                    
                }
                
            });
        }

        private void Fire(Player player, int count)
        {
            for (int i = 0; i < count; i++)
            {
                WindProjectileParticle windProjectileParticle = MonoGenericPool<WindProjectileParticle>.Pop();
                        
                Vector3 direction = directions[i].normalized;
                Vector3 spawnOffset = direction * 2 + new Vector3(0, 0.5f, 0); 
                        
                windProjectileParticle.transform.position = player.GetPlayerTransform.position + spawnOffset;
                windProjectileParticle.SetDirection(direction);
            }
        }
    }
}