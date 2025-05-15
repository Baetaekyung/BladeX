using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "ShardMagicSkill", menuName = "SO/Skill/Purple/ShardMagicSkill")]
    public class ShardMagicSkill : SkillData
    {
        [SerializeField] private float damage;
        private bool hasGeneratedText = false;
        
        public override void Initialize()
        {
            MonoGenericPool<ShardMagicParticle>.Initialize(skillParticle);
        }
    
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemyHealth health) && health.isDead)
                {
                    if (hasGeneratedText == false)
                    {
                        hasGeneratedText = true;
                        GenerateSkillText(true);
                    }
                    
                    ShardMagicParticle shardMagicParticle = MonoGenericPool<ShardMagicParticle>.Pop();
                    shardMagicParticle.SetDamage(damage + GetColorRatio());
                    shardMagicParticle.transform.forward = player.GetPlayerTransform.forward;
                    shardMagicParticle.transform.position = item.position + new Vector3(0,0.25f,0);
                }
            }
            
            hasGeneratedText = false;
        }
                
    }
}
