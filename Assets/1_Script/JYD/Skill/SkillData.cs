using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject
    {
        public string skillName;
        public Sprite skillIcon;
        public SkillType SkillType;
        public StatType StatType;
        public ColorType colorType;
        
        [Tooltip("¼º°ø È®·ü")][Range(1,100)] public int random;
        
        [TextArea] public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO skillParticle;
        
        public virtual void Initialize(){}

        public virtual void SkillUpdate(Player player, List<Transform> targets = null){}
        
        public abstract void UseSkill(Player player, Transform[] targets = null);
        
        public bool CheckSkill()
        {
            return Random.Range(0, 100) > random;
        }
        
    }
}