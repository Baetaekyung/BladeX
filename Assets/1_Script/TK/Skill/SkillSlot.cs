using System;
using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class SkillSlot : Slot
    {
        [SerializeField] private Sprite    originalImage;
        [SerializeField] private Image     skillIcon;
        //[SerializeField] private SkillType slotSkillType;
        
        private SkillUIDataSO _skillUIData;

        //public SkillType GetSkillType => slotSkillType;

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
        
                
        public override void SetSlotData<T>(T data)
        {
            _skillUIData = data as SkillUIDataSO;
            
            if (_skillUIData == default)
            {
                Debug.LogWarning("not matched data type, correct type is SkillUIDataSO");
                return;
            }
            
            SetSlotImage(_skillUIData.GetSkillIcon);
        }

        public override bool IsEmptySlot() => _skillUIData == null;

        private void OnValidate()
        {
            // if (gameObject.name.Contains("attack"))
            //     slotSkillType = SkillType.Attack;
            // else if (gameObject.name.Contains("parry"))
            //     slotSkillType = SkillType.Parry;
            // else if (gameObject.name.Contains("rolling"))
            //     slotSkillType = SkillType.Rolling;
            // else if (gameObject.name.Contains("debuff"))
            //     slotSkillType = SkillType.Debuff;
            // else if (gameObject.name.Contains("grow"))
            //     slotSkillType = SkillType.Grow;
        }
    }
}
