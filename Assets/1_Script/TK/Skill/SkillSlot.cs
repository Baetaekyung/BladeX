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

        private SkillManager SkillManager => SkillManager.Instance;
        public SkillType     GetSkillType => slotSkillType;
        public ColorType     GetColorType => colorType;

        public override void SetSlotImage(Sprite sprite)
        {
            (int r, int g, int b) rgb = ColorUtils.GetRGBColor(colorType);

            Color transperentcolor = new Color(rgb.r, rgb.g, rgb.b, 0.25f);
            Color opaqueColor      = new Color(rgb.r, rgb.b, rgb.b, 0.65f);

            skillIcon.sprite = sprite ? sprite      : originalImage;
            skillIcon.color  = sprite ? opaqueColor : transperentcolor;
        }
        
        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                _skillData = null;
                SetSlotImage(null);
                return;
            }

            if (SkillManager.currentSkillCount >= SkillManager.maxSkillCount)
                return;
            
            _skillData = data;
            SetSlotImage(_skillData.skillIcon);

            Player.Instance.GetEntityComponent<PlayerSkillController>().AddSkill(_skillData);

            SkillManager.SetSkillCountUI(SkillManager.currentSkillCount, SkillManager.maxSkillCount);

            SkillManager.currentSkillCount++;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_skillData == null)
                return;

            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            var slot = SkillManager.GetEmptyInvSlot();

            //Slot is full
            if (slot == default)
                return;

            Player.Instance.GetEntityComponent<PlayerSkillController>().RemoveSkill(_skillData);

            SkillManager.saveDatas.AddSkillToInventory(_skillData);
            SkillManager.saveDatas.RemoveSlotSkillData(_skillData);
            SkillManager.currentSkillCount--;

            slot.SetSlotData(_skillData);
            SetSlotData(null);

            SkillManager.SetSkillCountUI(SkillManager.currentSkillCount, SkillManager.maxSkillCount);
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
