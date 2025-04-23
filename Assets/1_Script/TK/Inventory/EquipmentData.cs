using System;
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
    
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject, IPlayerEquipable
    {
        public SerializableDictionary<StatType, float> statModifier = new();

        [SerializeField] public string partsName;
        [SerializeField] private string displayName;

        [HideInInspector]
        public string itemSerialCode; //스텟에 더할때 구별해주는 번호
        public Sprite equipmentIcon;

        public EquipmentSlotType slotType;
        public ColorType         colorType;
        public int               colorAdder;

        ColorType IPlayerEquipable.GetColor => colorType;
        Sprite IPlayerEquipable.GetSprite => equipmentIcon;
        string IPlayerEquipable.DisplayName => displayName;


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
