using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ParryExplosionSkill", menuName = "SO/Skill/Red/ParryExplosion")]
    public class ParryExplosionSkill : SkillData
    {
        public int skillDamage;
        public Vector2 explosionAdjustment;
        public float skillRadius;
        public LayerMask whatIsTarget;
                
        
        public override void Initialize()
        {
            MonoGenericPool<SmallExplosionParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player, Transform[] targets = null)
        {
            Vector3 explosionPosition = player.GetPlayerTransform.position +
                                        (player.GetPlayerTransform.forward * explosionAdjustment.x);
            explosionPosition.y = explosionPosition.y + player.GetPlayerTransform.position.y;
            
            if (targets == null || targets.Length == 0)
            {
                targets = Physics.OverlapSphere(explosionPosition, skillRadius, whatIsTarget).Select(x => x.transform).ToArray();
            }
            
            foreach (var item in targets)
            {
                if (CheckSkill() && item.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData();
                    actionData.damageAmount = skillDamage;
                    health.TakeDamage(actionData);

                    SmallExplosionParticle smallExplosionParticle = MonoGenericPool<SmallExplosionParticle>.Pop();
                    smallExplosionParticle.transform.position =
                        explosionPosition;
                }                
            }
            
        }
        
    }
}
