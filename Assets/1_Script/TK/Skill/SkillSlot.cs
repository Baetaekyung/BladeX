using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillSlot : SkillSlotBase
    {
        [SerializeField] private Sprite    originalImage;
        [SerializeField] private Image     skillIcon;
        [SerializeField] private SkillType slotSkillType;
        [SerializeField] private ColorType colorType;
        
        private SkillData _skillData;

        private SkillManager skillManager => SkillManager.Instance;
        public SkillType     GetSkillType => slotSkillType;
        public ColorType     GetColorType => colorType;

        public override void SetSlotImage(Sprite sprite)
        {
            Color transperantColor = new Color(1, 1, 1, 0.25f);

            skillIcon.sprite = sprite ? sprite      : originalImage;
            skillIcon.color  = sprite ? Color.white : transperantColor;
        }
        
        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                _skillData = null;
                SetSlotImage(null);
                return;
            }

            if (skillManager.currentSkillCount >= skillManager.maxSkillCount)
                return;
            
            _skillData = data;
            SetSlotImage(_skillData.skillIcon);

            Player.Instance.GetEntityComponent<PlayerSkillController>().AddSkill(_skillData);

            skillManager.currentSkillCount++;
            skillManager.SetSkillCountUI(skillManager.currentSkillCount, skillManager.maxSkillCount);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_skillData == null)
                return;

            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            var slot = skillManager.GetEmptyInvSlot();

            //Slot is full
            if (slot == default)
                return;

            Player.Instance.GetEntityComponent<PlayerSkillController>().RemoveSkill(_skillData);

            SkillManager.saveDatas.AddSkillToInventory(_skillData);
            SkillManager.saveDatas.RemoveSlotSkillData(_skillData);

            skillManager.currentSkillCount--;

            slot.SetSlotData(_skillData);
            SetSlotData(null);

            skillManager.SetSkillCountUI(skillManager.currentSkillCount, skillManager.maxSkillCount);
        }

        #region MouseEvents

        public override void OnPointerEnter(PointerEventData eventData)
        { 
            OnPointerEnterAction?.Invoke(eventData.position, _skillData); 
        }

        public override void OnPointerMove(PointerEventData eventData)
        { 
            OnPointerEnterAction?.Invoke(eventData.position, _skillData);
        }
        
        public override void OnPointerExit(PointerEventData eventData) 
        { 
            OnPointerEnterAction?.Invoke(Vector2.zero, null);
        }

        #endregion
        
        public override bool IsEmptySlot() => !_skillData;
    }
}
