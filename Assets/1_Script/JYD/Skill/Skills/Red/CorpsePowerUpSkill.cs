using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public class CorpsePowerUpSkill : SkillData
    {
        [Range(0.1f, 10f)] [SerializeField] private float skillRadius = 3;
        
        public override void Initialize()
        {
            
        }

        public override void UseSkill(Player player, Transform[] targets = null)
        {
            //empty    
        }

        public override void SkillUpdate(Player player, List<Transform> targets = null)
        {
            
        }
        
        
    }
}
