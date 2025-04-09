using DG.Tweening;
using Swift_Blade.Skill;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillIngredientSlot : SkillSlotBase
    {
        public static event Action<ColorType> OnSkillStageEvent;

        [SerializeField] private Image itemImage;
        [SerializeField] private Image colorInfoIcon;

        private SkillData _skillData;

        public SkillData GetSkillData => _skillData;

        public void DeleteSkillData()
        {
            //Remove from save data and updateDatas
            SkillManager.saveDatas.RemoveInventoryData(_skillData);
            SkillManager.Instance.UpdateDatas();

            SetSlotData(null);
        }

        public override void SetSlotImage(Sprite sprite)
        {
            itemImage.color  = sprite != null ? Color.white : Color.clear;
            itemImage.sprite = sprite != null ? sprite      : null;
        }

        public override void SetSlotData(SkillData data)
        {
            if (data == null)
            {
                SetSlotImage(null);

                _skillData = null;
                colorInfoIcon.color = new Color(1, 1, 1, 0.7f);

                return;
            }

            SetSlotImage(data.skillIcon);
            
            (int r, int g, int b) = ColorUtils.GetRGBColor(data.colorType);
            Color newColor = new Color(r, g, b, 0.7f);
            colorInfoIcon.color = newColor;

            _skillData = data;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            if (_skillData == null)
                return;

            if (SkillManager.Instance.GetEmptyMixSlot() == null)
            {
                Debug.Log("비어있는 MixSlot이 존재하지 않습니다.");
                return;
            }

            SkillSlotToMix slot = SkillManager.Instance.GetEmptyMixSlot();
            slot.SetSlotData(_skillData);

            OnSkillStageEvent?.Invoke(_skillData.colorType);

            SkillManager.saveDatas.AddSkillToInventory(_skillData);
            SetSlotData(null);

            SkillManager.Instance.UpdateDatas();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(Vector3.one * 1.03f, 0.5f);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(Vector3.one, 0.5f);
        }

        public override void OnPointerMove(PointerEventData eventData) { } //hmm..
                                                                           
        public override bool IsEmptySlot() => _skillData == null;
    }
}
