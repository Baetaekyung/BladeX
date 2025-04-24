using UnityEngine;

namespace Swift_Blade
{
    public static class EquipmentUtility
    {
        public static string GetTagToKorean(EquipmentTag tag)
        {
            string tagKorean = tag switch
            {
                EquipmentTag.NONE => "����",
                EquipmentTag.BARBARIAN => "�߸�",
                EquipmentTag.KNIGHT => "���",
                EquipmentTag.ROGUE => "����",
                EquipmentTag.DEMON => "�Ǹ�",
                EquipmentTag.DRAGON => "��",
                EquipmentTag.MUTANT => "��������",
                EquipmentTag.HOLY => "�ż���",
                EquipmentTag.UNHOLY => "�Ұ���",
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
