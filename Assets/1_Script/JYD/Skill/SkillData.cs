using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject
    {
        public string skillName;
        [FormerlySerializedAs("type")] public SkillType skillType;
        public Sprite skillIcon;
        [TextArea]public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO SkillEffectPrefab;
        
        public abstract void UseSkill(Transform transform);

        //For deep copy
        public SkillData Clone()
        {
            var skillData = Instantiate(this);
            skillData.skillName = skillName;
            skillData.skillType = skillType;
            skillData.skillIcon = skillIcon;
            skillData.skillDescription = skillDescription;
            skillData.SkillEffectPrefab = SkillEffectPrefab;

            return skillData;
        }
    }
}
