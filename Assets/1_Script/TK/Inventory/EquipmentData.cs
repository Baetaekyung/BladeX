using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public enum EquipmentSlotType
    {
        Head,
        Armor,
        Weapon, 
        Shield,
        Shoes
    }
    
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        public SerializableDictionary<StatType, float> statModifier
            = new SerializableDictionary<StatType, float>();
        [field: SerializeField] public EquipmentChannelSO EventChannel { get; private set; }
        [HideInInspector] public string                   itemSerialCode; //스텟에 더할때 구별해주는 번호
        public Sprite                                     equipmentIcon;

        public EquipmentSlotType slotType;
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if(itemSerialCode == String.Empty)
                itemSerialCode = Guid.NewGuid().ToString();
        }

#endif
    }
}
