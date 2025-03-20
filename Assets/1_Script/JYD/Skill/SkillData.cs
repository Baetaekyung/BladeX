using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject
    {
        public string skillName;
        public SkillType type;
        public Sprite skillIcon;
        [TextArea] public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO SkillEffectPrefab;
        
        public virtual void Initialize(){}
        
        public abstract void UseSkill(Transform transform);
        
    }
}