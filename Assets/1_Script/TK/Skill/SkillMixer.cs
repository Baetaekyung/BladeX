using DG.Tweening;
using Swift_Blade.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillMixer : MonoBehaviour
    {
        private const float COLOR_RESULT_SHOW_DELAY = 4f;

        [SerializeField] private SkillTable           skillTable;
        [SerializeField] private SkillIngredientSlot  leftSlot;
        [SerializeField] private SkillIngredientSlot  rightSlot;
        [SerializeField] private UIEffect_ColorChange eff;
        [SerializeField] private Image resultImage;

        private SkillData skillDataOnStage1;
        private SkillData skillDataOnStage2;

        private Tween _tween;

        private List<ColorType> ingredientColorTypes = new List<ColorType>();

        private void Start()
        {
            leftSlot.SetSlotData(null);
            rightSlot.SetSlotData(null);

            SkillSlotToMix.OnSkillStageEvent      += HandleResultColorAdd;
            SkillIngredientSlot.OnSkillStageEvent += HandleResultColorRemove;

            resultImage.color = Color.clear;
        }

        private void OnDisable()
        {
            SkillIngredientSlot.OnSkillStageEvent -= HandleResultColorAdd;
            SkillSlotToMix.OnSkillStageEvent -= HandleResultColorRemove;
        }

        private void HandleResultColorAdd(ColorType colorType)
        {
            ingredientColorTypes.Add(colorType);
        }

        private void HandleResultColorRemove(ColorType colorType)
        {
            ingredientColorTypes.Remove(colorType);

            if(IsReadyToMix() == false)
            {
                if (_tween != null)
                    _tween.Kill();

                resultImage.color = Color.clear;
                return;
            }
        }

        public void MixSkill()
        {
            if (IsReadyToMix() == false)
            {
                PopupManager.Instance.LogMessage("섞을 스킬을 등록하여 주세요.");
                return;
            }

            List<ColorType> containsColor = new List<ColorType>();

            ColorType leftType = leftSlot.GetSkillData.colorType;
            ColorType rightType = rightSlot.GetSkillData.colorType;

            containsColor.Add(leftType);
            containsColor.Add(rightType);

            ColorType mixedColorType = ColorUtils.GetColor(containsColor);

            if (mixedColorType == ColorType.RED 
                || mixedColorType == ColorType.GREEN 
                || mixedColorType == ColorType.BLUE)
            {
                PopupManager.Instance.LogMessage("스킬이 섞일 수 없습니다.");
                FailToMix();

                return;
            }

            AnimSkillMixed(mixedColorType, () =>
            {
                SkillData randomSkill = skillTable.GetRandomSkill(mixedColorType);

                if (randomSkill != null)
                {
                    SkillManager.Instance.TryAddSkillToInventory(randomSkill);
                }
                else
                    return;

                resultImage.color = Color.clear;;

                SkillManager.Instance.UpdateDatas();

                Color color = ColorUtils.GetCustomColor(mixedColorType);
                string colorText = ColorUtils.ColorText(randomSkill.skillName, color);

                eff.Blink(0.5f, () => PopupManager.Instance.LogMessage(
                    $"스킬을 얻었습니다 [{colorText}]"));
            });

            ingredientColorTypes.Clear();
            leftSlot.DeleteSkillData();
            rightSlot.DeleteSkillData();

            skillDataOnStage1 = null;
            skillDataOnStage2 = null;
        }

        private void AnimSkillMixed(ColorType resultColorType, Action callback = null)
        {
            Color resultColor = ColorUtils.GetColorRGBUnity(resultColorType);
            Color currentColor = resultImage.color;

            if (_tween != null)
                _tween.Kill();

            _tween = DOVirtual.Color(currentColor, resultColor, COLOR_RESULT_SHOW_DELAY,
                (cor) => resultImage.color = cor).OnComplete(() => callback?.Invoke());

            DOVirtual.Float(0, 1f, COLOR_RESULT_SHOW_DELAY, (f) => eff.SetEff(resultColor * 0.4f, f))
                .OnComplete(() => eff.SetEff(Color.clear, 1)).SetEase(Ease.InBack);
            DOVirtual.Float(0, 1f, COLOR_RESULT_SHOW_DELAY, (f) => eff.SetAlpha(f))
                .OnComplete(() => eff.SetAlpha(0)).SetEase(Ease.InBack);

            resultImage.color = resultColor;
        }

        private void FailToMix()
        {
            SkillManager.saveDatas.AddSkillToInventory(skillDataOnStage1);
            SkillManager.saveDatas.AddSkillToInventory(skillDataOnStage2);

            skillDataOnStage1 = null;
            skillDataOnStage2 = null;

            leftSlot.SetSlotData(null);
            rightSlot.SetSlotData(null);

            SkillManager.Instance.UpdateDatas();
        }

        public SkillIngredientSlot GetEmptyIngredientSlot()
        {
            if (leftSlot.IsEmptySlot())
                return leftSlot;
            else if(rightSlot.IsEmptySlot())
                return rightSlot;

            return null;
        }

        public void SetSkillData(SkillData skillData)
        {
            if(skillDataOnStage1 == null)
            {
                skillDataOnStage1 = skillData;
            }
            else if(skillDataOnStage2 == null)
            {
                skillDataOnStage2 = skillData;
            }
        }

        public bool IsReadyToMix() => leftSlot.IsEmptySlot() == false && rightSlot.IsEmptySlot() == false;
    }
}
