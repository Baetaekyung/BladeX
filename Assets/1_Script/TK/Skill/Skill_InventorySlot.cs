using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class Skill_InventorySlot : Slot
    {
        [SerializeField]
        private Image         skillIconImage;
        private SkillUIDataSO _skillUIData;
        
        public override void SetSlotData<T>(T data)
        {
            _skillUIData = data as SkillUIDataSO;

            if (_skillUIData == default)
            {
                Debug.LogWarning("not matched data type, correct type is SkillUIDataSO");
                return;
            }
            
            SetSlotImage(_skillUIData?.GetSkillIcon);
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

        public override bool IsEmptySlot() => _skillUIData == null;
    }
}
