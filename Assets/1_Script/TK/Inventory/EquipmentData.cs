using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [Serializable] [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<StatType, float> statModifier
            = new SerializableDictionary<StatType, float>();
        [field: SerializeField] public EquipmentChannelSO EventChannel { get; private set; }
        public string itemSerialCode; //���ݿ� ���Ҷ� �������ִ� ��ȣ
        public Sprite equipmentIcon;
        
        public void HandleStatAdder(PlayerStatCompo playerStat)
        {
            if (playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is valid component");
                return;
            }

            if (statModifier.Count == 0)
                return;
            
            foreach (var stat in statModifier)
            {
                //Key�� StatType, Value�� ModifyValue
                playerStat.AddModifier(stat.Key, itemSerialCode, stat.Value);
            }
        }

        public void HandleStatRemover(PlayerStatCompo playerStat)
        {
            if (playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is valid component");
                return;
            }

            if (statModifier.Count == 0)
                return;
            
            foreach (var stat in statModifier)
            {
                //Key�� StatType
                playerStat.RemoveModifier(stat.Key, itemSerialCode);
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(itemSerialCode == String.Empty)
                itemSerialCode = Guid.NewGuid().ToString();
        }

#endif
    }
}
