using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        COMMON,
        RARE,
        UNIQUE,
        EPIC,
        END = 99
    }
    
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        public SerializableDictionary<StatType, float> statModifier = new();

        public List<EquipmentTag> tags;
        public EquipmentRarity    rarity;

        public string partsName;

        [HideInInspector]
        public string itemSerialCode; //스텟에 더할때 구별해주는 번호
        public Sprite equipmentIcon;

        public EquipmentSlotType slotType;
        public ColorType         colorType;
        public int               colorAdder;

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
