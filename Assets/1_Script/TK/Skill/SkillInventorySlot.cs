using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillInventorySlot : SkillSlotBase
    {
        [SerializeField]
        private Image     skillIconImage;
        private SkillData _skillData;
        
        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                _skillData = null;
                SetSlotImage(null);
                return;
            }
            
            _skillData = data;
            SetSlotImage(data?.skillIcon);
        }

        public override void SetSlotImage(Sprite sprite)
        {
            if (sprite == null)
            {
                skillIconImage.color = Color.clear;
            }
            else
            {
                skillIconImage.color = Color.white;
                skillIconImage.sprite = sprite;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            if (_skillData == null)
                return;

            var slot = SkillManager.Instance.GetEmptySkillSlot(_skillData.type);

            //½½·ÔÀÌ °¡µæÂü
            if (slot == default)
            {
                Debug.Log("Slot is full");
            }
            else
            {
                SkillManager.saveDatas.AddSkillToSlot(_skillData);
                SkillManager.saveDatas.RemoveInvenSkillData(_skillData);
                
                slot.SetSlotData(_skillData);
                SetSlotData(null);
            }
        }
        
        public override bool IsEmptySlot() => _skillData == null;
    }
}
