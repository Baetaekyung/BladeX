using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class SkillUIManager : MonoSingleton<SkillUIManager>
    {
        [SerializeField] private List<SkillSlot>           skill_slots;
        [SerializeField] private List<Skill_InventorySlot> inv_slots;
        [SerializeField] private SkillUISaveSO             skillUISaveData;
        
        private static SkillUISaveSO saveDatas;
        private static bool          InitOnce = false;

        private void OnEnable()
        {
            if (InitOnce == false)
            {
                saveDatas = skillUISaveData.Clone();
                InitOnce = true;
            }
            
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            InitializeSlot(inv_slots); 
            InitializeSlot(skill_slots);
            
            //�ʱ�ȭ �Ŀ� ��������
            LoadUIData();
        }

        private void LoadUIData()
        {
            if (saveDatas.invUiData.Count > 0)
            {
                for (short i = 0; i < saveDatas.invUiData.Count; i++)
                    GetEmptyInvSlot().SetSlotData(saveDatas.invUiData[i]);
            }

            if (saveDatas.skillSlotUiData.Count > 0)
            {
                for (short i = 0; i < saveDatas.skillSlotUiData.Count; i++)
                {
                    // SkillType type = saveDatas.skillSlotUiData[i].GetSkillType;
                    // GetEmptySkillSlot(type).SetSlotData(saveDatas.skillSlotUiData[i]);
                }
            }
        }

        private void InitializeSlot<T>(List<T> slots) where T : Slot
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmptySlot())
                    slot.SetSlotImage(null);
            }
        }

        public Skill_InventorySlot GetEmptyInvSlot()
        {
            var invSlot = inv_slots.FirstOrDefault(slot => slot.IsEmptySlot());

            if (invSlot == default)
            {
                Debug.Log("Skill Inventory is Full!!");
                return default;
            }

            return invSlot;
        }

        // public SkillSlot GetEmptySkillSlot(SkillType skillType)
        // {
        //     var skillSlot = skill_slots
        //         .FindAll(slot => slot.IsEmptySlot()) //�� ��ų ���� ã�Ƽ�
        //         .FirstOrDefault(slot => slot.GetSkillType == skillType); //�� �� ���� Ÿ���� ��������
        //
        //     if (skillSlot == default)
        //     {
        //         Debug.Log($"{skillType.ToString()} type skill slot is Full");
        //         return default;
        //     }
        //
        //     return skillSlot;
        // }
    }
}
