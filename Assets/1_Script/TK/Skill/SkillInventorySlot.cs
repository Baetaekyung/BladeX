using Swift_Blade.Skill;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillInventorySlot : SkillSlotBase
    {
        [SerializeField] private Image skillIconImage;

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

            //������ ������
            if (slot == default)
            {
                string typeToKorean = _skillData.type switch
                {
                    SkillType.Attack => "����",
                    SkillType.Dead => "���",
                    SkillType.Hit => "�ǰ�",
                    SkillType.Parry => "�и�",
                    SkillType.Rolling => "������",
                    _ => "???"
                };

                var textPopup = PopupManager.Instance.GetPopupUI(PopupType.Text) as TextPopup;
                textPopup?.SetText($"{typeToKorean}Ÿ���� ��ų ������ �� á���ϴ�.");
                
                PopupManager.Instance.DelayPopup(PopupType.Text, 1f,
                    () => { PopupManager.Instance.PopDown(PopupType.Text); });
            }
            else if (SkillManager.Instance.CanAddSkill)
            {
                SkillManager.saveDatas.AddSkillToSlot(_skillData);
                SkillManager.saveDatas.RemoveInvenSkillData(_skillData);

                slot.SetSlotData(_skillData);
                SetSlotData(null);
            }
            else
            {
                var textPopup = PopupManager.Instance.GetPopupUI(PopupType.Text) as TextPopup;
                textPopup?.SetText("��ų�� ���� �� �� ����� �� �����ϴ�");
                
                PopupManager.Instance.DelayPopup(PopupType.Text, 1f,
                    () => { PopupManager.Instance.PopDown(PopupType.Text); });
            }
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

        public override bool IsEmptySlot() => _skillData == null;
    }
}