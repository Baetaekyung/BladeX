using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject
    {
        public string skillName;
        public SkillType type;
        public Sprite skillIcon;
        [TextArea] public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO skillParticle;
        
        public virtual void Initialize(){}
        
        public abstract void UseSkill(Player player,Transform[] targets = null);
        
    }
}