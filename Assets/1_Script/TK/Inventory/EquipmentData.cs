using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        public SerializableDictionary<StatType, float> statModifier
            = new SerializableDictionary<StatType, float>();
        [field: SerializeField] public EquipmentChannelSO EventChannel { get; private set; }
        public string itemSerialCode; //���ݿ� ���Ҷ� �������ִ� ��ȣ
        public Sprite equipmentIcon;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(itemSerialCode == String.Empty)
                itemSerialCode = Guid.NewGuid().ToString();
        }

#endif
    }
}
