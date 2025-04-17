using Swift_Blade.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(menuName = "SO/Skill/Skill_Table")]
    public class SkillTable : ScriptableObject
    {
        public List<SkillData> redSkillDatas       = new List<SkillData>();
        public List<SkillData> greenSkillDatas     = new List<SkillData>();
        public List<SkillData> blueSkillDatas      = new List<SkillData>();
        public List<SkillData> yellowSkillDatas    = new List<SkillData>();
        public List<SkillData> purpleSkillDatas    = new List<SkillData>();
        public List<SkillData> turquoiseSkillDatas = new List<SkillData>();

        public SkillData GetRandomSkill(ColorType colorType)
        {
            int index = 0;
            switch (colorType)
            {
                case ColorType.RED:
                    index = Random.Range(0, redSkillDatas.Count);
                    return redSkillDatas[index];

                case ColorType.GREEN:
                    index = Random.Range(0, greenSkillDatas.Count);
                    return greenSkillDatas[index];

                case ColorType.BLUE:
                    index = Random.Range(0, blueSkillDatas.Count);
                    return blueSkillDatas[index];

                case ColorType.YELLOW:
                    index = Random.Range(0, yellowSkillDatas.Count);
                    return yellowSkillDatas[index];

                case ColorType.PURPLE:
                    index = Random.Range(0, purpleSkillDatas.Count);
                    return purpleSkillDatas[index];

                case ColorType.TURQUOISE:
                    index = Random.Range(0, turquoiseSkillDatas.Count);
                    return turquoiseSkillDatas[index];
                default: break;
            }

            Debug.LogWarning($"Any data not exist, skill color {colorType.ToString()}");
            return null;
        }
    }
}
