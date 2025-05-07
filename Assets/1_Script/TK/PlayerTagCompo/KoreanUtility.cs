using UnityEngine;

namespace Swift_Blade
{
    public static class KoreanUtility
    {
        public static string GetStatTypeToKorean(StatType statType)
        {
            string statToKorean = statType switch
            {
                StatType.HEALTH => "ü��",
                StatType.DAMAGE => "���ݷ�",
                StatType.ATTACKSPEED => "���ݼӵ�",
                StatType.MOVESPEED => "�̵��ӵ�",
                StatType.DASH_INVINCIBLE_TIME => "�����⹫���ð�",
                StatType.PARRY_CHANCE => "Ư���ɷ����ӽð�",
                StatType.CRITICAL_CHANCE => "ġ��ŸȮ��",
                StatType.CRITICAL_DAMAGE => "ġ��Ÿ������%",
                _ => "Error"
            };

            return statToKorean;
        }

        public static string GetTagToKorean(EquipmentTag tag)
        {
            string tagKorean = tag switch
            {
                EquipmentTag.NONE => "����",
                EquipmentTag.BARBARIAN => "�߸�",
                EquipmentTag.KNIGHT => "���",
                EquipmentTag.ROGUE => "����",
                EquipmentTag.DRAGON => "��",
                EquipmentTag.MUTANT => "��������",
                EquipmentTag.HOLY => "�ż���",
                EquipmentTag.ALL => "����",
                _ => "����"
            };

            return tagKorean;
        }

        public static string GetRarityColorText(EquipmentRarity rarity)
        {
            string colorRarity = rarity switch
            {
                EquipmentRarity.NONE   => "<color=white>��޾���</color>",
                EquipmentRarity.COMMON => "<color=white>���: ���</color>",
                EquipmentRarity.RARE   => "<color=white>���: ���</color>",
                EquipmentRarity.UNIQUE => "<color=white>���: ����ũ</color>",
                EquipmentRarity.EPIC   => "<color=white>���: �ְ�</color>",
                EquipmentRarity.END    => "����",
                _ => "����"
            };

            return colorRarity;
        }
    }
}
