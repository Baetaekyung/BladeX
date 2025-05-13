using System.Collections.Generic;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using System.Linq;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryExplosionSkill", menuName = "SO/Skill/Red/ParryExplosion")]
    public class ParryExplosionSkill : SkillData
    {
        [Space]
        
        public float fireTime;
        public float fireDamage;
        
        public Vector2 explosionAdjustment;
        public float skillRadius;
        public LayerMask whatIsTarget;
                
        public override void Initialize()
        {
            MonoGenericPool<SmallExplosionParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player,  IEnumerable<Transform> targets = null)
        {
            Vector3 explosionPosition = player.GetPlayerTransform.position +
                                        (player.GetPlayerTransform.forward * explosionAdjustment.x);
            explosionPosition.y +=  player.GetPlayerTransform.position.y + explosionAdjustment.y;
            
            if (targets == null || !targets.Any())
            {
                targets = Physics.OverlapSphere(explosionPosition, skillRadius, whatIsTarget).Select(x => x.transform).ToArray();
            }
            
            foreach (var item in targets)
            {
                if (TryUseSkill() && item.TryGetComponent(out BaseEnemy enemy))
                {
                    enemy.GetEffectController().SetFire(fireDamage, fireTime);
                    
                    SmallExplosionParticle smallExplosionParticle = MonoGenericPool<SmallExplosionParticle>.Pop();
                    smallExplosionParticle.transform.position = explosionPosition;
                }
            }
                        
        }
        
    }
}
