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
        [Space]
        [SerializeField] private float radius;
        [SerializeField] private float slowDuration;
        [SerializeField] private float defaultSlowValue;
        [SerializeField] private float minSlowValue;
        [SerializeField] private LayerMask whatIsEnemy;
        
        private bool hasGeneratedText = false;
        
        public override void Initialize()
        {
            MonoGenericPool<AreaTyphoonParticle>.Initialize(skillParticle);
        }
        
        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            GenerateSkillText(true);
            
            targets = Physics.OverlapSphere(player.GetPlayerTransform.position, radius, whatIsEnemy)
                .Select(c => c.transform);
            
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemy enemy))
                {
                    float animationSpeed = Mathf.Max(minSlowValue, defaultSlowValue);
                    enemy.GetEffectController().SetSlow(animationSpeed,slowDuration);
                    
                    MonoGenericPool<AreaTyphoonParticle>.Pop().transform.position = player.GetPlayerTransform.position + new Vector3(0,0.6f,0);
                    
                    IceSmokeParticle iceSmokeParticle = MonoGenericPool<IceSmokeParticle>.Pop();
                    iceSmokeParticle.transform.SetParent(enemy.transform);
                    iceSmokeParticle.transform.position = enemy.transform.position + new Vector3(0,0.25f,0);
                    
                    if (hasGeneratedText == false)
                    {
                        GenerateSkillText(true);
                        hasGeneratedText = true;
                    }
                }
                
                hasGeneratedText = true;
            }
            
        }
        
    }
}