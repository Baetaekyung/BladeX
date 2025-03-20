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

        protected override void Awake()
        {
            base.Awake();

            if (!InitOnce)
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
            
            //Load
            for (short i = 0; i < saveDatas.invUiData.Count; i++)
                GetEmptyInvSlot().SetSlotData(saveDatas.invUiData[i]);
            for (short i = 0; i < saveDatas.skillSlotUiData.Count; i++)
            {
                SkillType type = saveDatas.skillSlotUiData[i].GetSkillType;
                GetEmptySkillSlot(type).SetSlotData(saveDatas.skillSlotUiData[i]);
            }
        }

        private void InitializeSlot<T>(List<T> slots) where T : Slot
        {
            foreach (var slot in slots)
                if (slot.IsEmptySlot())
                    slot.SetSlotImage(null);
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

        public SkillSlot GetEmptySkillSlot(SkillType skillType)
        {
            var skillSlot = skill_slots
                .FindAll(slot => slot.IsEmptySlot()) //빈 스킬 슬롯 찾아서
                .FirstOrDefault(slot => slot.GetSkillType == skillType); //그 중 같은 타입의 슬롯으로

            if (skillSlot == default)
            {
                Debug.Log($"{skillType.ToString()} type skill slot is Full");
                return default;
            }

            return skillSlot;
        }
    }
}
