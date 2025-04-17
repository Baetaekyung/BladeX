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

                    if (redSkillDatas.Count == 0)
                        break;

                    return redSkillDatas[index];

                case ColorType.GREEN:
                    index = Random.Range(0, greenSkillDatas.Count);

                    if (greenSkillDatas.Count == 0)
                        break;

                    return greenSkillDatas[index];

                case ColorType.BLUE:
                    index = Random.Range(0, blueSkillDatas.Count);

                    if (blueSkillDatas.Count == 0)
                        break;

                    return blueSkillDatas[index];

                case ColorType.YELLOW:
                    index = Random.Range(0, yellowSkillDatas.Count);

                    if (yellowSkillDatas.Count == 0)
                        break;

                    return yellowSkillDatas[index];

                case ColorType.PURPLE:
                    index = Random.Range(0, purpleSkillDatas.Count);

                    if (purpleSkillDatas.Count == 0)
                        break;

                    return purpleSkillDatas[index];

                case ColorType.TURQUOISE:
                    index = Random.Range(0, turquoiseSkillDatas.Count);

                    if (turquoiseSkillDatas.Count == 0)
                        break;

                    return turquoiseSkillDatas[index];
                default: break;
            }

            Debug.LogWarning($"Any data not exist, skill color {colorType.ToString()}");
            return null;
        }
    }
}
