using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        public static bool        IsNewGame = false;
        
        private void OnEnable()
        {
            if (IsNewGame == false)
            {
                saveDatas = skillSaveData.Clone();
                IsNewGame = true;
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
            currentSkillCount = 0;

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
            int i;
            for (i = 0; i < saveDatas.inventoryData.Count; i++)
                GetEmptyInvSlot().SetSlotData(saveDatas.inventoryData[i]);

            for (i = 0; i < saveDatas.inventoryData.Count; i++)
                GetEmptyMixSlot().SetSlotData(saveDatas.inventoryData[i]);

            for (i = 0; i < saveDatas.skillSlotData.Count; i++)
            {
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
            int i;
            for(i = 0; i < slots.Count(); i++)
                slots.ElementAt(i).SetSlotData(null);

            //foreach (var slot in slots)
            //    slot.SetSlotData(null);
        }

        public void SetSkillCountUI(int current, int max)
        {
            maxSkillText.text = $"{current} / {max}";
        }
        
        public SkillInventorySlot GetEmptyInvSlot()
        {
            SkillInventorySlot inventorySlot = null;

            int i = 0;
            for(i = 0; i < inv_slots.Count; i++)
            {
                if (inv_slots[i].IsEmptySlot())
                {
                    inventorySlot = inv_slots[i];
                    break;
                }
            }

            if (inventorySlot == null)
            {
                PopupManager.Instance.LogMessage("인벤토리 슬롯이 가득 찼습니다.");
                return inventorySlot;
            }

            return inventorySlot;
        }

        public SkillSlot GetEmptySkillSlot()
        {
            SkillSlot skillSlot = null;

            int i = 0;
            for(i = 0; i < skill_slots.Count; i++)
            {
                if (skill_slots[i].IsEmptySlot())
                {
                    skillSlot = skill_slots[i];
                    break;
                }
            }

            if (skillSlot == null)
            {
                Debug.LogWarning("빈 Skill slot을 찾을 수 없다.");
                return skillSlot;
            }
        
            return skillSlot;
        }

        public SkillSlotToMix GetEmptyMixSlot()
        {
            SkillSlotToMix skillMixSlot = null;

            int i = 0;
            for(i = 0; i < mix_slots.Count; i++)
            {
                if (mix_slots[i].IsEmptySlot())
                {
                    skillMixSlot = mix_slots[i];
                    break;
                }
            }

            if (skillMixSlot == null)
            {
                Debug.LogWarning("빈 SkillMix slot을 찾을 수 없다.");
                return skillMixSlot;
            }

            return skillMixSlot;
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
