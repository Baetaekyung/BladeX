using UnityEngine;

namespace Swift_Blade
{
    public static class KoreanUtility
    {
        public static string GetStatTypeToKorean(StatType statType)
        {
            string statToKorean = statType switch
            {
                StatType.HEALTH => "체력",
                StatType.DAMAGE => "공격력",
                StatType.ATTACKSPEED => "공격속도",
                StatType.MOVESPEED => "이동속도",
                StatType.DASH_INVINCIBLE_TIME => "구르기무적시간",
                StatType.PARRY_CHANCE => "특수능력지속시간",
                StatType.CRITICAL_CHANCE => "치명타확률",
                StatType.CRITICAL_DAMAGE => "치명타데미지%",
                _ => "Error"
            };

            return statToKorean;
        }

        public static string GetTagToKorean(EquipmentTag tag)
        {
            string tagKorean = tag switch
            {
                EquipmentTag.NONE => "없음",
                EquipmentTag.BARBARIAN => "야만",
                EquipmentTag.KNIGHT => "기사",
                EquipmentTag.ROGUE => "도둑",
                EquipmentTag.DRAGON => "용",
                EquipmentTag.MUTANT => "돌연변이",
                EquipmentTag.HOLY => "신성한",
                EquipmentTag.ALL => "전부",
                _ => "오류"
            };

            return tagKorean;
        }

        public static string GetRarityColorText(EquipmentRarity rarity)
        {
            string colorRarity = rarity switch
            {
                EquipmentRarity.NONE   => "<color=white>등급없음</color>",
                EquipmentRarity.COMMON => "<color=white>등급: 평범</color>",
                EquipmentRarity.RARE   => "<color=white>등급: 희귀</color>",
                EquipmentRarity.UNIQUE => "<color=white>등급: 유니크</color>",
                EquipmentRarity.EPIC   => "<color=white>등급: 최고</color>",
                EquipmentRarity.END    => "오류",
                _ => "오류"
            };

            return colorRarity;
        }
    }
}
