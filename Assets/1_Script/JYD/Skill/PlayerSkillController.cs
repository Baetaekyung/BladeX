using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Attack,
    Rolling,
    Special,//c key
    Hit,
    Dead
}
namespace Swift_Blade.Skill
    {
        public class PlayerSkillController : MonoBehaviour, IEntityComponent,IEntityComponentStart
        {
            private Player _player;
            
            public event Action<Player,Transform[]> OnAttackEventSkill;
            public event Action<Player,Transform[]> OnRollingEventSkill;
            public event Action<Player,Transform[]> OnSpecialEventSkill;
            public event Action<Player,Transform[]> OnHitEventSkill;
            public event Action<Player,Transform[]> OnDeadEventSkill;

            [SerializeField] private SkillData[] skillDatas;
            
            [SerializeField] private List<SkillData> currentSkillList;
            
            private Dictionary<SkillType, Action<Player,Transform[]>> skillEvents;
            private ushort maxSlotCount = 4;
            private ushort slotCount = 0;

            public bool canDrawGizmo;
            
            private void Awake()
            {
                skillEvents = new Dictionary<SkillType, Action<Player,Transform[]>>()
                {
                    { SkillType.Attack, OnAttackEventSkill },
                    { SkillType.Rolling, OnRollingEventSkill },
                    { SkillType.Special, OnSpecialEventSkill },
                    { SkillType.Hit, OnHitEventSkill },
                    { SkillType.Dead, OnDeadEventSkill }
                };
                
                StartCoroutine(SKillUpdateRoutine());
            }
            
            private IEnumerator SKillUpdateRoutine()
            {
                while (true)
                {
                    if (currentSkillList == null) continue;
                    
                    for (int i = 0; i < currentSkillList.Count; i++)
                    {
                        currentSkillList[i].SkillUpdate(_player);
                    }
                    yield return new WaitForSeconds(0.02f);
                }
            }
            
            /*private void Update()
            {
                foreach (var item in currentSkillList)
                {
                    item.SkillUpdate(_player);
                }
            }*/

            private void OnDrawGizmos()
            {
                if(canDrawGizmo== false)return;

                foreach (var item in skillDatas)
                {
                    item.Render();   
                }
            }

            public void EntityComponentAwake(Entity entity)
            {
                _player = entity as Player;
            }
            
            public void EntityComponentStart(Entity entity)
            {
                SkillManager.Instance.LoadSkillData();
                
                InitializeSkill();
            }

            private void InitializeSkill()
            {
                foreach (var skill in currentSkillList)
                {
                    skill.Initialize();
                }
            }

            public void AddSkill(SkillData skillData)
            {
                if (slotCount >= maxSlotCount) return;
                
                if (skillEvents.ContainsKey(skillData.skillType))
                {
                    skillEvents[skillData.skillType] += skillData.UseSkill;
                    currentSkillList.Add(skillData);
                    ++slotCount;
                    
                    skillData.Initialize();
                }
            }

            public void RemoveSkill(SkillData skillData)
            {
                if (skillEvents.ContainsKey(skillData.skillType) && skillEvents[skillData.skillType] != null)
                {
                    skillEvents[skillData.skillType] -= skillData.UseSkill;
                    currentSkillList.Remove(skillData);
                    --slotCount;
                }
            }
                
            public void UseSkill(SkillType type,Transform[] targets = null)
            {
                if (skillEvents.ContainsKey(type))
                {
                    skillEvents[type]?.Invoke(_player,targets);
                }
            }
            
            
        }
}
        

