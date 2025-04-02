using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillInventorySlot : SkillSlotBase
    {
        [SerializeField] private Image skillIconImage;

        private SkillData skillData;

        public override void SetSlotData(SkillData data)
        {
            skillData = data ? data : null;
            SetSlotImage(data ? data.skillIcon : null);
        }

        public override void SetSlotImage(Sprite sprite)
        {
            Color newColor = Color.white;

            if(skillData)
            {
                (int r, int g, int b) = ColorUtils.GetRGBColor(skillData.colorType);
                newColor = new Color(r * 0.7f, g * 0.7f, b * 0.9f, 1f);
                
            }

            skillIconImage.color  = sprite ? newColor : Color.clear;
            skillIconImage.sprite = sprite;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            if (skillData == null)
                return;

            var slot = SkillManager.Instance.GetEmptySkillSlot(
                skillData.skillType,
                skillData.colorType);

            //������ ������
            if (slot == default)
            {
                string typeToKorean = skillData.skillType switch
                {
                    SkillType.Attack  => "����",
                    SkillType.Dead    => "���",
                    SkillType.Hit     => "�ǰ�",
                    SkillType.Parry   => "�и�",
                    SkillType.Rolling => "������",
                    _ => "???"
                };

                PopupManager.Instance.LogMessage($"{typeToKorean}Ÿ���� ��ų ������ �� á���ϴ�.");
            }
            else if (SkillManager.Instance.CanAddSkill)
            {
                SkillManager.saveDatas.AddSkillToSlot(skillData);
                SkillManager.saveDatas.RemoveInvenSkillData(skillData);

                slot.SetSlotData(skillData);
                SetSlotData(null);
            }
            else
                PopupManager.Instance.LogMessage("��ų�� ���� �� �� ����� �� �����ϴ�");
        }

        #region MouseEvents

        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterAction?.Invoke(eventData.position, skillData);
        }

        public override void OnPointerMove(PointerEventData eventData)
        {
            OnPointerEnterAction?.Invoke(eventData.position, skillData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            OnPointerEnterAction?.Invoke(Vector2.zero, null);
        }

        #endregion

        public override bool IsEmptySlot() => skillData == null;
    }
}