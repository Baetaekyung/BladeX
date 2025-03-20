using Swift_Blade.Skill;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SkillUIData", menuName = "SO/Skill_UI_Data")]
    public class SkillUIDataSO : ScriptableObject
    {
        [TextArea]
        [SerializeField] private string    description;
        [SerializeField] private string    skillName;
        [SerializeField] private Sprite    skillIcon;
        [SerializeField] private SkillType skillType;
                                      
        //SO 데이터 수정 못하고 가져오기만 하게끔
        public string    GetDescription => description;
        public string    GetSkillName   => skillName;
        public Sprite    GetSkillIcon   => skillIcon;
        public SkillType GetSkillType   => skillType;
    }                                   
}                                       
