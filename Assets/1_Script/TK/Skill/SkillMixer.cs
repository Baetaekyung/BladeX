using DG.Tweening;
using Swift_Blade.Skill;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillMixer : MonoBehaviour
    {
        private const float COLOR_RESULT_SHOW_DELAY = 2.5f;

        [SerializeField] private SkillTable          skillTable;
        [SerializeField] private SkillIngredientSlot leftSlot;
        [SerializeField] private SkillIngredientSlot rightSlot;
        [SerializeField] private Image resultImage;

        private bool _canMix;

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

            ColorType getColorType = ColorUtils.GetColor(ingredientColorTypes);

            if(getColorType == ColorType.RED || getColorType == ColorType.BLUE || getColorType == ColorType.GREEN)
            {
                resultImage.color = Color.clear;
                return;
            }

            (int r, int g, int b) = ColorUtils.GetRGBColor(getColorType);
            Color resultColor = new Color(r, g, b, 0.9f);
            Color currentColor = resultImage.color;

            if (_tween != null)
                _tween.Kill();

            _tween = DOVirtual.Color(currentColor, resultColor, COLOR_RESULT_SHOW_DELAY,
                (cor) => resultImage.color = cor).OnComplete(() => _canMix = true);

            resultImage.color = resultColor;
        }

        private void HandleResultColorRemove(ColorType colorType)
        {
            ingredientColorTypes.Remove(colorType);
            _canMix = false;

            if(ingredientColorTypes.Count < 2)
            {
                if (_tween != null)
                    _tween.Kill();

                resultImage.color = Color.clear;
                return;
            }

            //ingredient color is mixed color
            //ColorType getColorType = ColorUtils.GetColor(ingredientColorTypes);

            //if (getColorType == ColorType.RED || getColorType == ColorType.BLUE || getColorType == ColorType.GREEN)
            //{
            //    resultImage.color = Color.clear;
            //    return;
            //}
            
            //(int r, int g, int b) = ColorUtils.GetRGBColor(getColorType);
            //Color resultColor = new Color(r, g, b, 0.9f);

            //resultImage.color = resultColor;
        }

        public void MixSkill()
        {
            if (_canMix == false)
                return;

            if (IsReadyToMix() == false)
            {
                PopupManager.Instance.LogMessage("���� ��ų�� ����Ͽ� �ּ���.");
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
                PopupManager.Instance.LogMessage("��ų�� ���� �� �����ϴ�.");

                FailToMix();

                return;
            }

            leftSlot.DeleteSkillData();
            rightSlot.DeleteSkillData();

            if (_tween != null)
                _tween.Kill();

            resultImage.color = Color.clear;

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
