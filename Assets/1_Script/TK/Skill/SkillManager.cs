using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class SkillManager : MonoSingleton<SkillManager>
    {
        [SerializeField] private List<SkillSlot>           skill_slots;
        [SerializeField] private List<SkillInventorySlot>  inv_slots;
        [SerializeField] private SkillSaveSO               skillSaveData;
        
        public static SkillSaveSO  saveDatas;
        private static bool        InitOnce = false;
        
        private void OnEnable()
        {
            if (InitOnce == false)
            {
                saveDatas = skillSaveData.Clone();
                InitOnce = true;
            }
            
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            InitializeSlot(inv_slots); 
            InitializeSlot(skill_slots);
        }

        private void LoadData()
        {
            if (saveDatas.inventoryData.Count > 0)
            {
                for (ushort i = 0; i < saveDatas.inventoryData.Count; i++)
                {
                    GetEmptyInvSlot().SetSlotData(saveDatas.inventoryData[i]);
                    Debug.Log($"{i} index inventorySlot was setted to {saveDatas.inventoryData[i]}");
                }
            }

            if (saveDatas.skillSlotData.Count > 0)
            {
                for (ushort i = 0; i < saveDatas.skillSlotData.Count; i++)
                {
                    SkillType type = saveDatas.skillSlotData[i].type;
                    GetEmptySkillSlot(type).SetSlotData(saveDatas.skillSlotData[i]);
                }
            }
        }

        //Call after player initialized
        public void LoadSkillData()
        {
            LoadData();
        }

        private void InitializeSlot<T>(List<T> slots) where T : SkillSlotBase
        {
            foreach (var slot in slots)
                slot.SetSlotData(null);
        }

        public SkillInventorySlot GetEmptyInvSlot()
        {
            var invSlot = inv_slots.FirstOrDefault(slot => slot.IsEmptySlot());

            if (invSlot == default)
            {
                Debug.Log("Skill Inventory is Full!!");
                return default;
            }

            return invSlot;
        }

        public SkillSlot GetEmptySkillSlot(SkillType skillType)
        {
            var skillSlot = skill_slots.FirstOrDefault(slot => 
                slot.GetSkillType == skillType && slot.IsEmptySlot()); 
        
            if (skillSlot == default)
            {
                Debug.Log($"{skillType.ToString()} type skill slot is Full");
                return default;
            }
        
            return skillSlot;
        }

        //If player get skill, skill needs to go to inv right? 
        public bool TryAddSkillToInventory(SkillData skillData)
        {
            var inventorySlot = GetEmptyInvSlot();

            if (inventorySlot == default)
            {
                Debug.Log("inventory is full");
                return false;
            }
            
            inventorySlot.SetSlotData(skillData);
            saveDatas.AddSkillToInventory(skillData);
            Debug.Log("Added Item");
            return true;
        }
    }
}
