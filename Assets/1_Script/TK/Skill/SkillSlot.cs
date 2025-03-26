using Swift_Blade.Skill;
using Swift_Blade.UI;
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
        
        private SkillData _skillData;

        public SkillType GetSkillType => slotSkillType;

        public override void SetSlotImage(Sprite sprite)
        {
            if (sprite == null)
            {
                skillIcon.color = new Color(1, 1, 1, 0.3f);
                skillIcon.sprite = originalImage;
                return;
            }

            skillIcon.sprite = sprite;
            skillIcon.color  = Color.white;
        }
        
        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                _skillData = null;
                SetSlotImage(null);
                return;
            }

            if (SkillManager.Instance.currentSkillCount >= SkillManager.Instance.maxSkillCount)
                return;
            
            _skillData = data;
            SetSlotImage(_skillData.skillIcon);
            Player.Instance.GetEntityComponent<PlayerSkillController>().AddSkill(_skillData);
            SkillManager.Instance.currentSkillCount++;
            
            SkillManager.Instance.SetSkillCountUI(
                SkillManager.Instance.currentSkillCount, 
                SkillManager.Instance.maxSkillCount);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            if (_skillData == null)
                return;

            var slot = SkillManager.Instance.GetEmptyInvSlot();

            //ΩΩ∑‘¿Ã ∞°µÊ¬¸
            if (slot == default)
            {
                Debug.Log("Slot is full");
            }
            else
            {
                Player.Instance.GetEntityComponent<PlayerSkillController>().RemoveSkill(_skillData);
                
                SkillManager.saveDatas.AddSkillToInventory(_skillData);
                SkillManager.saveDatas.RemoveSlotSkillData(_skillData);
                SkillManager.Instance.currentSkillCount--;
                
                slot.SetSlotData(_skillData);
                SetSlotData(null);
                
                SkillManager.Instance.SetSkillCountUI(
                    SkillManager.Instance.currentSkillCount, 
                    SkillManager.Instance.maxSkillCount);
            }
        }

        #region MouseEvents

        public override void OnPointerEnter(PointerEventData eventData)
        { OnPointerEnterAction?.Invoke(eventData.position, _skillData); }

        public override void OnPointerMove(PointerEventData eventData)
        { OnPointerEnterAction?.Invoke(eventData.position, _skillData); }
        
        public override void OnPointerExit(PointerEventData eventData) 
        { OnPointerEnterAction?.Invoke(Vector2.zero, null); }

        #endregion
        
        public override bool IsEmptySlot() => _skillData == null;
    }
}
