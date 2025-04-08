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
        [SerializeField] private List<SkillSlotToMix>     mix_slots;
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
        }

        private void Start()
        {
            PopupManager.Instance.OnPopUpOpenOrClose += UpdateDatas;
        }

        private void OnDisable()
        {
            SkillSlotBase.OnPointerEnterAction -= HandleCreateInfoUI;
        }

        private void InitializeSlots()
        {
            InitializeSlot(inv_slots);
            InitializeSlot(skill_slots);
            InitializeSlot(mix_slots);
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

        public void UpdateDatas()
        {
            InitializeSlots();
            LoadData();
        }

        private void LoadData()
        {
            if (saveDatas.inventoryData.Count > 0)
            {
                for (int i = 0; i < saveDatas.inventoryData.Count; i++)
                    GetEmptyInvSlot().SetSlotData(saveDatas.inventoryData[i]);
            }

            if (saveDatas.inventoryData.Count > 0)
            {
                for (int i = 0; i < saveDatas.inventoryData.Count; i++)
                    GetEmptyMixSlot().SetSlotData(saveDatas.inventoryData[i]);
            }

            if (saveDatas.skillSlotData.Count > 0)
            {
                for (int i = 0; i < saveDatas.skillSlotData.Count; i++)
                    GetEmptySkillSlot().SetSlotData(saveDatas.skillSlotData[i]);
            }
        }

        //Call after player initialized
        public void LoadSkillData()
        {
            LoadData();
        }

        private void InitializeSlot<T>(IEnumerable<T> slots) where T : SkillSlotBase
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
                PopupManager.Instance.LogMessage("인벤토리 슬롯이 가득 찼습니다.");
                return default;
            }

            return invSlot;
        }

        public SkillSlot GetEmptySkillSlot()
        {
            var skillSlot = skill_slots.FirstOrDefault(slot => slot.IsEmptySlot()); 
        
            if (skillSlot == default)
            {
                Debug.LogWarning("빈 Skill slot을 찾을 수 없다.");
                return default;
            }
        
            return skillSlot;
        }

        public SkillSlotToMix GetEmptyMixSlot()
        {
            var mixSlot = mix_slots.FirstOrDefault(slot => slot.IsEmptySlot());

            if (mixSlot == default)
            {
                Debug.LogWarning("빈 SkillMix slot을 찾을 수 없다.");
                return default;
            }

            return mixSlot;
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
