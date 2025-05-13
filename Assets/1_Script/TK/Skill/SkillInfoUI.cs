using DG.Tweening;
using Swift_Blade.Skill;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class SkillInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private float originX;
        [SerializeField] private float maxPosX;
        [SerializeField] private float moveTime;

        private RectTransform _rTrm;

        private void Awake()
        {
            _rTrm = GetComponent<RectTransform>();
        }

        public void SetSkillInfo(SkillData skillData)
        {
            if (skillData != null)
            {
                _rTrm.DOAnchorPosX(maxPosX, moveTime)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            else
            {
                _rTrm.DOAnchorPosX(originX, moveTime)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            string text = skillData ? skillData.skillName : string.Empty;
            if (skillData != null)
            {
                Color color = ColorUtils.GetCustomColor(skillData.colorType);
                text = ColorUtils.ColorText(text, color);
            }

            nameText.text = text;
            descriptionText.text = skillData ? skillData.skillDescription : string.Empty;
        }
    }
}
