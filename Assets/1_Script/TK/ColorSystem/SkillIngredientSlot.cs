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
        private const float COLOR_CHANGE_DURATION = 1f;
        public static event Action<ColorType> OnSkillStageEvent;

        [SerializeField] private Image itemImage;
        [SerializeField] private Image colorInfoIcon;

        private SkillData _skillData;
        private Tween     _tween;

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

                if (_tween != null)
                    _tween.Kill();

                Color curCol = colorInfoIcon.color;

                _tween = DOVirtual.Float(0, 1, COLOR_CHANGE_DURATION,
                    (t) => colorInfoIcon.color = Color.Lerp(curCol, new Color(1, 1, 1, 0.7f), t));

                return;
            }

            SetSlotImage(data.skillIcon);
            
            Color newColor = ColorUtils.GetCustomColor(data.colorType);
            Color currentColor = colorInfoIcon.color;

            if (_tween != null)
                _tween.Kill();

            _tween = DOVirtual.Float(0, 1, COLOR_CHANGE_DURATION,
                (t) => colorInfoIcon.color = Color.Lerp(currentColor, newColor, t));

            _skillData = data;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right
                && eventData.button != PointerEventData.InputButton.Left)
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
