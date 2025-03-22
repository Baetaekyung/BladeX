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

        private ushort maxSlotCount = 4;
        private ushort slotCount = 0;
        
        private void Start()
        {
            foreach (var item in testSkillSO)
            {
                AddSkill(item);
            }
        }
        
        private void OnDestroy()
        {
            foreach (var item in testSkillSO)
            {
                RemoveSkill(item);
            }
        }

        public void EntityComponentAwake(Entity entity) { }
        
        public void AddSkill(SkillData skillData)
        {
            if(slotCount >= maxSlotCount)return;   
            
            switch (skillData.skillType)
            {
                case SkillType.Attack:
                    OnAttackEventSkill += skillData.UseSkill;
                    break;
                case SkillType.Rolling:
                    OnRollingEventSkill += skillData.UseSkill;
                    break;
                case SkillType.Parry:
                    OnParryEventSkill += skillData.UseSkill;
                    break;
                case SkillType.Hit:
                    OnHitEventSkill += skillData.UseSkill;
                    break;
                case SkillType.Dead:
                    OnDeadEventSkill += skillData.UseSkill;
                    break;
            }
            ++slotCount;
        }
        
        public void RemoveSkill(SkillData skillData)
        {
            switch (skillData.skillType)
            {
                case SkillType.Attack:
                    OnAttackEventSkill -= skillData.UseSkill;
                    break;
                case SkillType.Rolling:
                    OnRollingEventSkill-= skillData.UseSkill;
                    break;
                case SkillType.Parry:
                    OnParryEventSkill-= skillData.UseSkill;
                    break;
                case SkillType.Hit:
                    OnHitEventSkill-= skillData.UseSkill;
                    break;
                case SkillType.Dead:
                    OnDeadEventSkill -= skillData.UseSkill;
                    break;
            }
            
            --slotCount;
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
