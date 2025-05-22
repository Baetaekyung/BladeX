using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Attack,
    Rolling,
    Parry,
    Hit,
    Dead,
    Shield,
    SpeedUp,
    None
}
namespace Swift_Blade.Skill
{
    public class PlayerSkillController : MonoBehaviour, IEntityComponent, IEntityComponentStart
    {
        private Player _player;

        public event Action<Player, IEnumerable<Transform>> OnAttackEventSkill;
        public event Action<Player, IEnumerable<Transform>> OnRollingEventSkill;
        public event Action<Player, IEnumerable<Transform>> OnSpecialEventSkill;
        public event Action<Player, IEnumerable<Transform>> OnHitEventSkill;
        public event Action<Player, IEnumerable<Transform>> OnDeadEventSkill;


        [SerializeField] private List<SkillData> currentSkillList;

        private Dictionary<SkillType, Action<Player, IEnumerable<Transform>>> skillEvents;
        private ushort maxSlotCount = 4;
        private ushort slotCount = 0;

        public bool canDrawGizmo;

        private WaitForSeconds skillUpdateWait;
        
        private void Awake()
        {
            skillUpdateWait = new WaitForSeconds(0.001f);
            
            skillEvents = new Dictionary<SkillType, Action<Player, IEnumerable<Transform>>>()
                {
                    { SkillType.Attack, OnAttackEventSkill },
                    { SkillType.Rolling, OnRollingEventSkill },
                    { SkillType.Parry, OnSpecialEventSkill },
                    { SkillType.Hit, OnHitEventSkill },
                    { SkillType.Shield, OnHitEventSkill },
                    { SkillType.SpeedUp, OnHitEventSkill },
                    { SkillType.Dead, OnDeadEventSkill }
                };

            StartCoroutine(SKillUpdateRoutine());
        }

        private IEnumerator SKillUpdateRoutine()
        {
            while (true)
            {
                if (currentSkillList == null)
                    continue;

                for (int i = 0; i < currentSkillList.Count; i++)
                {
                    currentSkillList[i].SkillUpdate(_player);
                }

                yield return skillUpdateWait;
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
            if (canDrawGizmo == false) return;

            foreach (var item in currentSkillList)
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
                skill.SetPlayerStatCompo(_player.GetPlayerStat);
                skill.Initialize();
            }
        }

        public void ResetSkill()
        {
            foreach (var item in currentSkillList)
            {
                item.ResetSkill();
            }
        }
        
        public void AddSkill(SkillData skillData)
        {
            if (slotCount >= maxSlotCount) return;

            if (currentSkillList.Contains(skillData))
                return;
            
            if (skillEvents.ContainsKey(skillData.skillType))
            {
                skillEvents[skillData.skillType] += skillData.UseSkill;
                currentSkillList.Add(skillData);
                ++slotCount;
                
                skillData.Initialize();
                skillData.SetPlayerStatCompo(_player.GetPlayerStat);
            }
        }

        public void RemoveSkill(SkillData skillData)
        {
            if (skillEvents.ContainsKey(skillData.skillType) && skillEvents[skillData.skillType] != null)
            {
                skillEvents[skillData.skillType] -= skillData.UseSkill;
                currentSkillList.Remove(skillData);
                --slotCount;
                
                skillData.ResetSkill();
            }
        }

        public void UseSkill(SkillType type, IEnumerable<Transform> targets = null)
        {
            if (skillEvents.ContainsKey(type))
            {
                skillEvents[type]?.Invoke(_player, targets);
            }
        }

        
    }
}


