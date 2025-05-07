using DG.Tweening;
using Swift_Blade.Skill;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillSlotToMix : SkillSlotBase
    {
        private const float COLOR_CHANGE_DURATION = 0.6f;
        public static event Action<ColorType> OnSkillStageEvent;

        [SerializeField] private Image skillIcon;
        [SerializeField] private Image skillColorImage;

        private SkillMixer _skillMixer;
        private SkillData  _skillData;

        private Tween _tween;

        private void Awake()
        {
            _skillMixer = GetComponentInParent<SkillMixer>();
        }

        public override void SetSlotData(SkillData data)
        {
            if(data == null)
            {
                SetSlotImage(null);
                _skillData = null;

                if (_tween != null)
                    _tween.Kill();

                Color currentColor = skillColorImage.color;
                _tween = DOVirtual.Float(0, 1, COLOR_CHANGE_DURATION,
                    (t) => skillColorImage.color = Color.Lerp(currentColor, Color.clear, t));

                return;
            }

            Color newColor = ColorUtils.GetCustomColor(data.colorType);
            Color curColor = skillColorImage.color;

            if(_tween != null)
                _tween.Kill();

            _tween = DOVirtual.Float(0, 1, COLOR_CHANGE_DURATION,
                (t) => skillColorImage.color = Color.Lerp(curColor, newColor, t));

            SetSlotImage(data.skillIcon);

            _skillData = data;
        }

        public override void SetSlotImage(Sprite sprite)
        {
            if (sprite == null)
            {
                skillIcon.color = Color.clear;
                skillIcon.sprite = null;
                return;
            }

            skillIcon.sprite = sprite;
            skillIcon.color = Color.white;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (IsEmptySlot() == true)
                return;

            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            if (_skillData.colorType == ColorType.PURPLE ||
                _skillData.colorType == ColorType.TURQUOISE ||
                _skillData.colorType == ColorType.YELLOW ||
                _skillData.colorType == ColorType.BLACK)
                return;

            if (_skillMixer.IsReadyToMix())
                return;

            _skillMixer.GetEmptyIngredientSlot().SetSlotData(_skillData);
            _skillMixer.SetSkillData(_skillData);

            OnSkillStageEvent?.Invoke(_skillData.colorType);
            SkillManager.saveDatas.RemoveInventoryData(_skillData);

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
            //Animation
            transform.DOKill();
            transform.DOScale(Vector3.one, 0.5f);
        }

        public override void OnPointerMove(PointerEventData eventData) { } //hmm..

        public override bool IsEmptySlot() => _skillData == null;
    }
}
