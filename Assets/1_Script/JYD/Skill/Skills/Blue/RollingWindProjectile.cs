using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "RollingWindProjectileSkill", menuName = "SO/Skill/Blue/WindProjectile")]
    public class RollingWindProjectile : SkillData
    {
        [SerializeField] private int skillCount;
        [Range(1,4)][SerializeField] private int projectileCount = 1;
        
        private readonly Vector3[] directions = new Vector3[4];
        private int skillCounter = 0;
                
        public override void Initialize()
        {
            MonoGenericPool<WindProjectileParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            if(CheckSkill() == false)return;
            
            if (directions == null || directions.Length != skillCount)
            {
                directions[0] = player.GetPlayerTransform.forward;
                directions[1] = player.GetPlayerTransform.right;
                directions[2] = -player.GetPlayerTransform.right;
                directions[3] = -player.GetPlayerTransform.forward;
            }
            
            ++skillCounter;
            if (skillCounter >= skillCount)
            {
                for (int i = 0; i  < projectileCount; i++)
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