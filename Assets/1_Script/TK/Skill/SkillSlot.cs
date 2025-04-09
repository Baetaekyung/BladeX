using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillSlot : SkillSlotBase
    {
        [SerializeField] private Image     skillIcon;
        [SerializeField] private Image     backgroundImage;
        
        private SkillData _skillData;

        private SkillManager skillManager => SkillManager.Instance;

        public override void SetSlotImage(Sprite sprite)
        {
            skillIcon.sprite = sprite ? sprite      : null;
            skillIcon.color  = sprite ? Color.white : Color.clear;
        }
        
        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                _skillData = null;
                backgroundImage.color = new Color(1, 1, 1, 0.6f);
                SetSlotImage(null);
                return;
            }

            if (skillManager.currentSkillCount >= skillManager.maxSkillCount)
                return;
            
            _skillData = data;
            SetSlotImage(data.skillIcon);

            Player.Instance.GetEntityComponent<PlayerSkillController>().AddSkill(data);

            skillManager.currentSkillCount++;
            skillManager.SetSkillCountUI(skillManager.currentSkillCount, skillManager.maxSkillCount);

            (int r, int g, int b) = ColorUtils.GetRGBColor(data.colorType);
            Color newColor = new Color(r, g, b, 0.4f);

            backgroundImage.color = newColor;
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
