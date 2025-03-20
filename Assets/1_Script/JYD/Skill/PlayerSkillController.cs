using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum SkillType
    {
        Attack,
        Rolling,
        Parry,
        Hit,
        Dead
    }


    namespace Swift_Blade.Skill
    {
        public class PlayerSkillController : MonoBehaviour, IEntityComponent,IEntityComponentStart
        {
            public event Action<Transform> OnAttackEventSkill;
            public event Action<Transform> OnRollingEventSkill;
            public event Action<Transform> OnParryEventSkill;
            public event Action<Transform> OnHitEventSkill;
            public event Action<Transform> OnDeadEventSkill;

            [SerializeField] private SkillData[] skillDatas;
            
            [SerializeField] private List<SkillData> currentSkillList;
            
            private Dictionary<SkillType, Action<Transform>> skillEvents;
            private ushort maxSlotCount = 4;
            private ushort slotCount = 0;
            
            private void Awake()
            {
                skillEvents = new Dictionary<SkillType, Action<Transform>>()
                {
                    { SkillType.Attack, OnAttackEventSkill },
                    { SkillType.Rolling, OnRollingEventSkill },
                    { SkillType.Parry, OnParryEventSkill },
                    { SkillType.Hit, OnHitEventSkill },
                    { SkillType.Dead, OnDeadEventSkill }
                };
                //currentSkillList = new List<SkillData>();
            }

            private void Start()
            {
                foreach (var item in skillDatas)
                {
                    AddSkill(item);
                }
            }

            private void OnDestroy()
            {
                
            }
            
            public void EntityComponentAwake(Entity entity)
            {
            }
            
            public void EntityComponentStart(Entity entity)
            {
                foreach (var skill in currentSkillList)
                {
                    skill.Initialize();
                }
            }
            
            public void AddSkill(SkillData skillData)
            {
                if (slotCount >= maxSlotCount) return;
            
                if (skillEvents.ContainsKey(skillData.type))
                {
                    skillEvents[skillData.type] += skillData.UseSkill;
                    currentSkillList.Add(skillData);
                    ++slotCount;
                }
            }

            public void RemoveSkill(SkillData skillData)
            {
                if (skillEvents.ContainsKey(skillData.type) && skillEvents[skillData.type] != null)
                {
                    skillEvents[skillData.type] -= skillData.UseSkill;
                    currentSkillList.Remove(skillData);
                    --slotCount;
                }
            }
                
            public void UseSkill(SkillType type)
            {
                if (skillEvents.ContainsKey(type))
                {
                    skillEvents[type]?.Invoke(transform);
                }
            }

            
        }
    
}
        

