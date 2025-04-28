using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum EquipmentSlotType
    {
        HEAD,
        ARMOR,
        WEAPON, 
        RING,
        SHOES
    }

    public enum EquipmentTag
    {
        NONE = 0,
        BARBARIAN,
        KNIGHT,
        ROGUE,
        DEMON,
        DRAGON,
        MUTANT,
        HOLY,
        UNHOLY,
        ALL = 99
    }

    public enum EquipmentRarity
    {
        NONE = 0,
        COMMON = 1,
        RARE = 2,
        UNIQUE = 3,
        EPIC = 4,
        END = 99
    }
    
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject, IPlayerEquipable
    {
        public SerializableDictionary<StatType, float> statModifier = new();

        public List<EquipmentTag> tags;
        public EquipmentRarity    rarity;

        [SerializeField] private string partsName;
        [SerializeField] private string displayName;

        [HideInInspector]
        public string itemSerialCode; //���ݿ� ���Ҷ� �������ִ� ��ȣ
        public Sprite equipmentIcon;

        public EquipmentSlotType slotType;
        public ColorType         colorType;
        public int               colorAdder;

        ColorType IPlayerEquipable.GetColor => colorType;
        Sprite IPlayerEquipable.GetSprite => equipmentIcon;
        string IPlayerEquipable.DisplayName => displayName;

        public string GetPartsName => partsName;
        private void OnValidate()
        {
            if (String.IsNullOrEmpty(itemSerialCode))
                itemSerialCode = Guid.NewGuid().ToString();

            string dataName = name.ToString();
            dataName = dataName.Substring(2, dataName.Length - 8);
            partsName = dataName;
        }

    }
}
