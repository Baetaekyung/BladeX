using Swift_Blade.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillMixer : MonoBehaviour
    {
        [SerializeField] private SkillTable          skillTable;
        [SerializeField] private SkillIngredientSlot leftSlot;
        [SerializeField] private SkillIngredientSlot rightSlot;
        [SerializeField] private Image resultImage;

        private SkillData skillDataOnStage1;
        private SkillData skillDataOnStage2;

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

            ColorType getColorType = ColorUtils.GetColor(ingredientColorTypes);

            if(getColorType == ColorType.RED || getColorType == ColorType.BLUE || getColorType == ColorType.GREEN)
            {
                resultImage.color = Color.clear;
                return;
            }

            (int r, int g, int b) = ColorUtils.GetRGBColor(getColorType);
            Color resultColor = new Color(r, g, b, 0.7f);

            resultImage.color = resultColor;
        }

        private void HandleResultColorRemove(ColorType colorType)
        {
            ingredientColorTypes.Remove(colorType);

            if(ingredientColorTypes.Count == 0)
            {
                resultImage.color = Color.clear;
                return;
            }

            ColorType getColorType = ColorUtils.GetColor(ingredientColorTypes);

            if (getColorType == ColorType.RED || getColorType == ColorType.BLUE || getColorType == ColorType.GREEN)
            {
                resultImage.color = Color.clear;
                return;
            }

            (int r, int g, int b) = ColorUtils.GetRGBColor(getColorType);
            Color resultColor = new Color(r, g, b, 0.7f);

            resultImage.color = resultColor;
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

            leftSlot.DeleteSkillData();
            rightSlot.DeleteSkillData();

            skillDataOnStage1 = null;
            skillDataOnStage2 = null;

            SkillManager.Instance.TryAddSkillToInventory(skillTable.GetRandomSkill(mixedColorType));

            SkillManager.Instance.UpdateDatas();
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
