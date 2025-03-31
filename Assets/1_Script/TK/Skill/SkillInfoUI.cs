using Swift_Blade.Skill;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class SkillInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetSkillInfo(SkillData skillData)
        {
            nameText.text        = skillData ? skillData.skillName : string.Empty;
            descriptionText.text = skillData ? skillData.skillDescription : string.Empty;
        }
    }
}
