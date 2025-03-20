using System;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public enum SkillType
    {
        Attack,
        Rolling,
        Parry,
        Hit,
        Dead
    }
    
    public class PlayerSkillController : MonoBehaviour,IEntityComponent
    {
        public event Action<Transform> OnAttackEventSkill;
        public event Action<Transform> OnRollingEventSkill;
        public event Action<Transform> OnParryEventSkill;
        public event Action<Transform> OnHitEventSkill;
        public event Action<Transform> OnDeadEventSkill;

        [SerializeField] private SkillData[] testSkillSO;
        
        private void Start()
        {
            foreach (var item in testSkillSO)
            {
                AddSkill(item);
            }
        }
        
        private void OnDestroy()
        {
            foreach (SkillType item in Enum.GetValues(typeof(SkillType)))
            {
                RemoveSkill(item);
            }
        }

        public void EntityComponentAwake(Entity entity) { }
        
        public void AddSkill(SkillData skillData)
        {
            switch (skillData.type)
            {
                case SkillType.Attack:
                    OnAttackEventSkill = skillData.UseSkill;
                    break;
                case SkillType.Rolling:
                    OnRollingEventSkill = skillData.UseSkill;
                    break;
                case SkillType.Parry:
                    OnParryEventSkill = skillData.UseSkill;
                    break;
                case SkillType.Hit:
                    OnHitEventSkill = skillData.UseSkill;
                    break;
                case SkillType.Dead:
                    OnDeadEventSkill = skillData.UseSkill;
                    break;
            }
        }
        
        public void RemoveSkill(SkillType type)
        {
            switch (type)
            {
                case SkillType.Attack:
                    OnAttackEventSkill = null;
                    break;
                case SkillType.Rolling:
                    OnRollingEventSkill = null;
                    break;
                case SkillType.Parry:
                    OnParryEventSkill = null;
                    break;
                case SkillType.Hit:
                    OnHitEventSkill = null;
                    break;
                case SkillType.Dead:
                    OnDeadEventSkill = null;
                    break;
            }
        }
        
        public void UseSkill(SkillType type)
        {
            switch (type)
            {
                case SkillType.Attack:
                    OnAttackEventSkill?.Invoke(transform);
                    break;
                case SkillType.Rolling:
                    OnRollingEventSkill?.Invoke(transform);
                    break;
                case SkillType.Parry:
                    OnParryEventSkill?.Invoke(transform);;
                    break;
                case SkillType.Hit:
                    OnHitEventSkill?.Invoke(transform);
                    break;
                case SkillType.Dead:
                    OnDeadEventSkill?.Invoke(transform);
                    break;
            }
        }
        

        
    }
}
