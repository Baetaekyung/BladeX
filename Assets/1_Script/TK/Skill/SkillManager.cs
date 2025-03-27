using System;
using System.Collections.Generic;
using System.Linq;
using Swift_Blade.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class SkillManager : MonoSingleton<SkillManager>
    {
        [SerializeField] private List<SkillSlot>          skill_slots;
        [SerializeField] private List<SkillInventorySlot> inv_slots;
        [SerializeField] private SkillSaveSO              skillSaveData;
        [SerializeField] private RectTransform            rootRect;
        [SerializeField] private TextMeshProUGUI          maxSkillText;
        [SerializeField] private Vector2                  skillInfoOffset;
        [SerializeField] private SkillInfoUI              infoUI;
        
        public int currentSkillCount = 0;
        public int maxSkillCount     = 4;

        public bool CanAddSkill => currentSkillCount < maxSkillCount;
        
        public static SkillSaveSO  saveDatas;
        private static bool        InitOnce = false;
        
        private void OnEnable()
        {
            if (InitOnce == false)
            {
                saveDatas = skillSaveData.Clone();
                InitOnce = true;
            }

            SkillSlotBase.OnPointerEnterAction += HandleCreateInfoUI;

            HandleCreateInfoUI(Vector2.zero, null);
            InitializeSlots();
        }

        private void OnDisable()
        {
            SkillSlotBase.OnPointerEnterAction -= HandleCreateInfoUI;
        }

        private void InitializeSlots()
        {
            InitializeSlot(inv_slots);
            InitializeSlot(skill_slots);
        }

        private void HandleCreateInfoUI(Vector2 screenPosition, SkillData skillData)
        {
            if (screenPosition == Vector2.zero || skillData == null)
            {
                infoUI.SetSkillInfo(null);
                infoUI.gameObject.SetActive(false);
                return;
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootRect,
                screenPosition,
                null,
                out var localPosition);

            infoUI.transform.localPosition = localPosition + skillInfoOffset;
            infoUI.SetSkillInfo(skillData);
            infoUI.gameObject.SetActive(true);
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

        public void SetSkillCountUI(int current, int max)
        {
            maxSkillText.text = $"{current} / {max}";
        }
        
        public SkillInventorySlot GetEmptyInvSlot()
        {
            var invSlot = inv_slots.FirstOrDefault(slot => slot.IsEmptySlot());

            if (invSlot == default)
            {
                PopupManager.Instance.LogMessage("스킬 인벤토리가 가득 찼습니다.");
                return default;
            }

            return invSlot;
        }

        public SkillSlot GetEmptySkillSlot(SkillType skillType)
        {
            var skillSlot = skill_slots.FirstOrDefault(slot => 
                slot.GetSkillType == skillType && slot.IsEmptySlot()); 
        
            if (skillSlot == default)
                return default;
        
            return skillSlot;
        }

        //If player get skill, skill needs to go to inv 
        public bool TryAddSkillToInventory(SkillData skillData)
        {
            var inventorySlot = GetEmptyInvSlot();

            if (inventorySlot == default)
                return false;
            
            inventorySlot.SetSlotData(skillData);
            saveDatas.AddSkillToInventory(skillData);

            return true;
        }
    }
}
