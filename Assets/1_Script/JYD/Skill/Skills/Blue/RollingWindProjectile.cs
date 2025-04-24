using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "RollingWindProjectileSkill", menuName = "SO/Skill/Blue/WindProjectile")]
    public class RollingWindProjectile : SkillData
    {
        [SerializeField] private int skillCount;
        
        private readonly Vector3[] directions = new Vector3[4];
        private int skillCounter = 0;

        private int projectileCount = 1;
        
        private const int MIN_SKILL_COUNT = 1;
        private const int MAX_SKILL_COUNT = 4;
        
        public override void Initialize()
        {
            MonoGenericPool<WindProjectileParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if(TryUseSkill() == false)return;
            
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
                for (int i = 0; i  < count; i++)
                {
                    WindProjectileParticle windProjectileParticle = MonoGenericPool<WindProjectileParticle>.Pop();
                    windProjectileParticle.transform.position = player.GetPlayerTransform.position;
                    windProjectileParticle.SetDirection(directions[i]);
                }
                
                skillCounter = 0;
            }
        }
        
    }
}