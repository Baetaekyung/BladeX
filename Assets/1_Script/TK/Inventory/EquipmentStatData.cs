using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentStatData : ScriptableObject
    {
        public string itemId; //Stat에 Add 해준것을 식별 할 수 있게하는 ID
        public SerializableDictionary<StatType, float> statAddValues = new SerializableDictionary<StatType, float>();
        public Sprite icon; //좀 꼬였다 나중에 고침
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (itemId != string.Empty) return;
            
            itemId = Guid.NewGuid().ToString();
        }

#endif
    }
}
