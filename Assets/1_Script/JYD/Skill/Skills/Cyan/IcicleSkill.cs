using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine;
using System.Linq;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "IcicleSkill",menuName = "SO/Skill/Cyan/IcicleSkill")]
    public class IcicleSkill : SkillData
    {
        [SerializeField] private float skillRadius;
        [SerializeField] private float skillDamage;
        [SerializeField] private float slowSpeed;
        [SerializeField] private float slowDuration;
        
        [SerializeField] private int icicleCount;
        [SerializeField] private LayerMask whatIsEnemy;
        
        private bool hasGeneratedText = false;
        
        public override void Initialize()
        {
            MonoGenericPool<IcicleParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            targets = Physics.OverlapSphere(player.GetPlayerTransform.position, skillRadius, whatIsEnemy).Select(x => x.transform);
            
            int i = 0;
            int count =  icicleCount + Mathf.FloorToInt(GetColorRatio());
            
            foreach (var item in targets)
            {
                if(i >= count)return;
                
                if (item.TryGetComponent(out BaseEnemyHealth health) && item.TryGetComponent(out BaseEnemy enemy))
                {
                    enemy.GetEffectController().SetSlow(slowSpeed,slowDuration);
                    
                    ActionData actionData = new ActionData();
                    actionData.stun = true;
                    actionData.damageAmount = skillDamage;
                    actionData.hitPoint = item.position + new Vector3(0, 0.25f, 0);
                    actionData.hurtType = 1;
                    health.TakeDamage(actionData);
                    
                    if (hasGeneratedText == false)
                    {
                        GenerateSkillText(true);
                        hasGeneratedText = true;
                    }
                    
                    MonoGenericPool<IcicleParticle>.Pop().transform.position = item.position;
                }
                
                i++;
            }
            
            hasGeneratedText = false;

        }
        
    }
}
