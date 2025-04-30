using System.Collections.Generic;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine;
using System.Linq;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "SlowAreaSkill",menuName = "SO/Skill/Cyan/SlowAreaSkill")]
    public class SlowAreaSkill : SkillData
    {
        public float disableTime = 3f;
        public float radius;
        public float minSlowValue;
        
        private float disableTimer;
        private bool useSkill;
        
        private AreaTyphoonParticle areaTyphoonParticle;
            
        public override void Initialize()
        {
            MonoGenericPool<AreaTyphoonParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            useSkill = true;
            disableTimer = 0;
            
            if (areaTyphoonParticle == null)
            {
                areaTyphoonParticle = MonoGenericPool<AreaTyphoonParticle>.Pop();
                areaTyphoonParticle.SetFollowTransform(player.GetPlayerTransform);
            }
            
            
        }
        
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (useSkill)
            {
                areaTyphoonParticle.SetFollowTransform(player.GetPlayerTransform);
                
                disableTimer += Time.deltaTime;
                
                targets = Physics.OverlapSphere(areaTyphoonParticle.transform.position, radius).Select(c => c.transform);
                foreach (var item in targets)
                {
                    float multipleAnimationSpeed = Mathf.Max(minSlowValue, (1 - GetColorRatio()));
                    
                    item.GetComponent<BaseEnemyAnimationController>().MultipleAnimationSpeed(multipleAnimationSpeed);
                }
                
                if (disableTimer >= disableTime)
                {
                    ResetSkill();
                }
            }
            
        }
        
        public override void ResetSkill()
        {
            useSkill = false;
            disableTimer = 0;
            MonoGenericPool<AreaTyphoonParticle>.Push(areaTyphoonParticle);
        }
        
        public override void DrawGizmo(Player player)
        {
            if(player != null)
                Gizmos.DrawWireSphere(player.transform.position , radius);
        }
        
    }
}
